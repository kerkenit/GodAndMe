using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GodAndMe.DependencyServices;
using GodAndMe.Extensions;
using GodAndMe.Interface;
using GodAndMe.Models;
using LocalAuthentication;
using Newtonsoft.Json;
using PCLStorage;
using SQLite;
using Xamarin.Forms;

namespace GodAndMe.Views
{
    public partial class SettingsPage : ContentPage
    {
        SQLiteConnection db;
        ISettings settings = DependencyService.Get<ISettings>();
        ITouchID TouchID = DependencyService.Get<ITouchID>();
        public SettingsPage()
        {
            InitializeComponent();
#if __IOS__
            var toolbarItem = new ToolbarItem();
            toolbarItem.IconImageSource = "hamburger.png";
            toolbarItem.Priority = -1;
            toolbarItem.Clicked += (object sender, EventArgs e) =>
            {
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = true;
            };
            ToolbarItems.Add(toolbarItem);
            tblCommon.Remove(MyName);

            if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {

                pnlImportAndExport.IsVisible = true;
            }
#elif __ANDROID__
            tblCommon.Remove(TouchIDEnabled);
            tblCommon.Remove(MyName);
#if DEBUG
            pnlImportAndExport.IsVisible = true;
#endif
#endif


            tblCommon.Title = string.Format("{0}-{1}", CommonFunctions.i18n("ApplicationTitle"), CommonFunctions.i18n("Settings"));


            MyName.Text = settings.GetYourName();
            MyName.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Text")
                {
                    settings.SetYourName(MyName.Text);
                }
            };
#if __IOS__
            switch (TouchID.GetLocalAuthType())
            {
                case LocalAuthType.TouchId:
                    TouchIDEnabled.Text = CommonFunctions.i18n("TouchIDEnabled");
                    break;
                case LocalAuthType.FaceId:
                    TouchIDEnabled.Text = CommonFunctions.i18n("FaceIDEnabled");
                    break;
                default:
                    TouchIDEnabled.IsEnabled = false;
                    break;
            }
#endif
            TouchIDEnabled.On = settings.GetTouchID();
            TouchIDEnabled.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                settings.SetTouchID(e.Value);
            };
            SetOrderByText();
            OrderBy.On = settings.GetContactSorting() == 1;
            OrderBy.OnChanged += (object sender, ToggledEventArgs e) =>
            {
                settings.SetContactSorting(e.Value ? 1 : 2);
                SetOrderByText();
            };
        }



        void Export_Clicked(object sender, EventArgs e)
        {
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();

            Base export = new Base();
            try
            {
                export.intention = db.Table<Intention>().ToList();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
#if DEBUG
                throw ex;
#endif
            }
            try
            {
                export.lent = db.Table<Lent>().ToList();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
#if DEBUG
                throw ex;
#endif
            }
            try
            {
                export.diary = db.Table<Diary>().ToList();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
#if DEBUG
                throw ex;
#endif
            }
            try
            {
                export.sins = db.Table<Sins>().ToList();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
#if DEBUG
                throw ex;
#endif
            }
            try
            {
                export.prayers = db.Table<Prayers>().ToList();
            }
#pragma warning disable CS0168 // Variable is declared but never used
            catch (SQLiteException ex)
#pragma warning restore CS0168 // Variable is declared but never used
            {
#if DEBUG
                throw ex;
#endif
            }

            //Task.Factory.StartNew((async () =>
            //{
            //    string url = await GetFilePath(export);
            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        IShare share = DependencyService.Get<IShare>();
            //        await share.Show(
            //           string.Format("Backup {0} op {1:D}", CommonFunctions.i18n("ApplicationTitle"), DateTime.Now),
            //           null,
            //           url,
            //           null
            //       );
            //    });
            //}), TaskCreationOptions.LongRunning);
            Device.BeginInvokeOnMainThread(async () =>
            {
                string result = CryptFile.Encrypt(JsonConvert.SerializeObject(export));
                IFileSystem fileSystem = FileSystem.Current;

                // Get the root directory of the file system for our application.
                IFolder rootFolder = fileSystem.LocalStorage;
                IFolder exportFolder = await rootFolder.CreateFolderAsync("export", CreationCollisionOption.OpenIfExists);
                IFile jsonFile = await exportFolder.CreateFileAsync(DateTime.Now.ToString("F") + CommonFunctions.EXTENSION, CreationCollisionOption.ReplaceExisting);
                await jsonFile.WriteAllTextAsync(result);

                IShare share = DependencyService.Get<IShare>();
                await share.Show(
                   string.Format("Backup {0} op {1:D}", CommonFunctions.i18n("ApplicationTitle"), DateTime.Now),
                   null,
                   jsonFile.Path,
                   null
               );
            });
        }


        //        async Task<string> GetFilePath(object export)
        //        {
        //            TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();

        //            try
        //            {
        //                IFile jsonFile = null;
        //                Device.BeginInvokeOnMainThread(async () =>
        //                {
        //                    IFileSystem fileSystem = FileSystem.Current;

        //                    // Get the root directory of the file system for our application.
        //                    IFolder rootFolder = fileSystem.LocalStorage;
        //                    IFolder exportFolder = await rootFolder.CreateFolderAsync("export", CreationCollisionOption.OpenIfExists);
        //                    jsonFile = await exportFolder.CreateFileAsync(DateTime.Now.ToString("F") + ".god", CreationCollisionOption.ReplaceExisting);
        //                });
        //                CryptFile cryptFile = new CryptFile(CommonFunctions.CRYPTOKEY);
        //                string result = cryptFile.Encrypt(JsonConvert.SerializeObject(export));
        //                if (result != null)
        //                {
        //                    await jsonFile.WriteAllTextAsync(result);

        //                    taskSource.SetResult(jsonFile.Path);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //#if DEBUG
        //                System.Diagnostics.Debug.WriteLine(ex);
        //                taskSource.SetResult(null);
        //#else
        //                 taskSource.SetResult(null);
        //#endif
        //    }
        //    return taskSource.Task.Result;
        //}

        void Import_Clicked(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                IDocument document = DependencyService.Get<IDocument>();

                string dataToShare = await document.GetFile();

                try
                {
                    ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenJson(dataToShare);
                }
                catch (Exception)
                {

                }
            });
        }

        private void SetOrderByText()
        {
            OrderBy.Text = string.Format("{0} {1}", CommonFunctions.i18n("OrderBy"), CommonFunctions.i18n(settings.GetContactSorting() == 1 ? "Lastname" : "Firstname").ToLower());
        }

        private bool touchIDisToggled;
        public bool TouchIDIsToggled
        {
            get
            {
                return touchIDisToggled;
            }
            set
            {
                touchIDisToggled = value;
                OnPropertyChanged("TouchIDEnabledIsToggled");
            }
        }

        private bool orderByIsToggled;
        public bool OrderByIsToggled
        {
            get { return orderByIsToggled; }
            set
            {
                orderByIsToggled = value;
                OnPropertyChanged("OrderByIsToggled");
            }
        }
    }
}
