using System;
using System.ComponentModel;
using System.Windows.Input;
using GodAndMe.Interface;
using GodAndMe.Models;
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
        public SettingsPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                var toolbarItem = new ToolbarItem();
                toolbarItem.Icon = "hamburger.png";
                toolbarItem.Priority = -1;
                toolbarItem.Clicked += (object sender, EventArgs e) =>
                {
                    ((MasterDetailPage)App.Current.MainPage).IsPresented = true;
                };
                ToolbarItems.Add(toolbarItem);
                tblCommon.Remove(MyName);
#if __IOS__
                if (UIKit.UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {

                    pnlImportAndExport.IsVisible = true;
                }
#endif
            }
            else
            {
                tblCommon.Remove(TouchIDEnabled);
                tblCommon.Remove(MyName);
            }

            tblCommon.Title = string.Format("{0}-{1}", CommonFunctions.i18n("ApplicationTitle"), CommonFunctions.i18n("Settings"));

            MyName.Text = settings.GetYourName();
            MyName.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == "Text")
                {
                    settings.SetYourName(MyName.Text);
                }
            };
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

        void btExport_Clicked(object sender, System.EventArgs e)
        {
            db = DependencyService.Get<IDatabaseConnection>().DbConnection();

            Base export = new Base();
            try
            {
                export.intention = db.Table<Intention>().ToList();
            }
            catch (SQLiteException)
            {

            }
            try
            {
                export.lent = db.Table<Lent>().ToList();
            }
            catch (SQLiteException)
            {

            }
            try
            {
                export.diary = db.Table<Diary>().ToList();
            }
            catch (SQLiteException)
            {

            }
            try
            {
                export.sins = db.Table<Sins>().ToList();
            }
            catch (SQLiteException)
            {

            }
            try
            {
                export.prayers = db.Table<Prayers>().ToList();
            }
            catch (SQLiteException)
            {

            }




            Device.BeginInvokeOnMainThread(async () =>
            {
                IFileSystem fileSystem = FileSystem.Current;

                // Get the root directory of the file system for our application.
                IFolder rootFolder = fileSystem.LocalStorage;
                IFolder exportFolder = await rootFolder.CreateFolderAsync("export", CreationCollisionOption.OpenIfExists);
                IFile jsonFile = await exportFolder.CreateFileAsync(DateTime.Now.ToString("F") + ".god", CreationCollisionOption.ReplaceExisting);
                await jsonFile.WriteAllTextAsync(JsonConvert.SerializeObject(export));
                IShare share = DependencyService.Get<IShare>();
                await share.Show(
                   string.Format("Backup {0} op {1:D}", CommonFunctions.i18n("ApplicationTitle"), DateTime.Now),
                   null,
                   jsonFile.Path,
                   null
               );
            });

        }

        void btImport_Clicked(object sender, EventArgs e)
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
