using System;
using System.Collections.Generic;
using GodAndMe.Extensions;
using GodAndMe.Interface;
using GodAndMe.Models;
using GodAndMe.Services;
using GodAndMe.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;


namespace GodAndMe.Views
{
    public partial class MainPage : MasterDetailPage
    {
        public List<MasterPageItem> menuList { get; set; }
        public MainPage()
        {
            InitializeComponent();
            IsPresented = false;

            menuList = new List<MasterPageItem>();

            // Creating our pages for menu navigation
            // Here you can define title for item, 
            // icon on the left side, and page that you want to open after selection
            // Adding menu items to menuList

            menuList.Add(new MasterPageItem
            {
                Title = CommonFunctions.i18n("MyIntentions"),
                IconSource = "tab_intention.png",
                TargetType = typeof(IntentionPage)
            });
            if (DateTime.Today > CommonFunctions.LentenPeriod.Start && DateTime.Today < CommonFunctions.LentenPeriod.End)
            {
                menuList.Add(new MasterPageItem
                {
                    Title = CommonFunctions.i18n("MyPeriodOfLent"),
                    IconSource = "tab_lent.png",
                    TargetType = typeof(LentPage)
                });
            }
            menuList.Add(new MasterPageItem
            {
                Title = CommonFunctions.i18n("MyReligiousDiary"),
                IconSource = "tab_diary.png",
                TargetType = typeof(DiaryPage)
            });
            menuList.Add(new MasterPageItem
            {
                Title = CommonFunctions.i18n("MySins"),
                IconSource = "tab_sins.png",
                TargetType = typeof(SinsPage)
            });
            menuList.Add(new MasterPageItem
            {
                Title = CommonFunctions.i18n("MyPrayers"),
                IconSource = "tab_prayers.png",
                TargetType = typeof(PrayersPage)
            });
            menuList.Add(new MasterPageItem
            {
                Title = CommonFunctions.i18n("Settings"),
                IconSource = "tab_settings.png",
                TargetType = typeof(SettingsPage)
            });
            menuList.Add(new MasterPageItem
            {
                Title = CommonFunctions.i18n("About"),
                IconSource = "tab_about.png",
                TargetType = typeof(AboutPage)
            });


            // Setting our list to be ItemSource for ListView in MainPage.xaml
            navigationDrawerList.ItemsSource = menuList;

            // Initial navigation, this can be used for our home page
            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(IntentionPage)));


            //masterPage.listView.ItemSelected += OnItemSelected;
            MasterBehavior = MasterBehavior.Split;
            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        public void OpenBase64(string base64)
        {
            string json = CryptFile.Decrypt(base64);
            object sender = null;
            try
            {
                if (json.Contains("Committed"))
                {
                    sender = JsonConvert.DeserializeObject<Sins>(json);
                }
                else
                {
                    sender = JsonConvert.DeserializeObject<Intention>(json);
                }
            }
            catch
            {
                sender = null;
            }

            try
            {
                if (sender != null && sender.GetType() == typeof(Intention))
                {
                    Intention intention = sender as Intention;
                    intention.Completed = false;
                    IDataStore<Intention> IntentionDataStore = new IntentionsDataStore();
                    IntentionDataStore.AddItemAsync(intention);
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(IntentionPage)));
                }
                else if (sender != null && sender.GetType() == typeof(Sins))
                {
                    Sins sin = sender as Sins;
                    sin.Confessed = false;
                    sin.Id = Guid.NewGuid().ToString();
                    IDataStore<Sins> SinsDataStore = new SinsDataStore();
                    SinsDataStore.AddItemAsync(sin);
                    Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(SinsPage)));
                }
            }
            catch
            {
                sender = null;
            }
            finally
            {
                if (sender == null)
                {

                }
            }
        }

        public void OpenJson(string json)
        {
            object sender = null;
            try
            {
                sender = JsonConvert.DeserializeObject(json, typeof(Base));
            }
            catch
            {
                sender = null;
            }
            if (sender == null)
            {
                try
                {
                    sender = JsonConvert.DeserializeObject(CryptFile.Decrypt(json), typeof(Base));
                }
                catch
                {
                    sender = null;
                }
            }
            if (sender == null)
            {
                try
                {
                    sender = JsonConvert.DeserializeObject(CryptFile.Decrypt_Legacy(json), typeof(Base));
                }
                catch
                {
                    sender = null;
                }
            }

            try
            {

                if (sender != null && sender.GetType() == typeof(Base) && (((Base)sender).intention != null || ((Base)sender).lent != null || ((Base)sender).diary != null || ((Base)sender).sins != null || ((Base)sender).prayers != null))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (await DisplayAlert(CommonFunctions.i18n("Import"), CommonFunctions.i18n("AllDataWillBeDeletedContinue"), CommonFunctions.i18n("Yes"), CommonFunctions.i18n("No")))
                        {
                            Base BaseJson = (Base)sender;
                            SQLite.SQLiteConnection db = DependencyService.Get<IDatabaseConnection>().DbConnection();

                            db.CreateTable<Intention>();
                            db.DeleteAll<Intention>();
                            db.InsertAll(BaseJson.intention);

                            db.CreateTable<Diary>();
                            db.DeleteAll<Diary>();
                            db.InsertAll(BaseJson.diary);

                            db.CreateTable<Lent>();
                            db.DeleteAll<Lent>();
                            db.InsertAll(BaseJson.lent);

                            db.CreateTable<Prayers>();
                            db.DeleteAll<Prayers>();
                            db.InsertAll(BaseJson.prayers);

                            db.CreateTable<Sins>();
                            db.DeleteAll<Sins>();
                            db.InsertAll(BaseJson.sins);
                            Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(IntentionPage)));
                        }
                    });
                }
            }
            catch
            {
                sender = null;
            }
        }

        void OnMenuItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                //masterPage.listView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
