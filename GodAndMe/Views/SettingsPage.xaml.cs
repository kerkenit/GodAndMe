using System;
using System.ComponentModel;
using GodAndMe.Interface;
using Xamarin.Forms;

namespace GodAndMe.Views
{
    public partial class SettingsPage : ContentPage
    {
        //public ICommand TouchIDToggle { get; set; }
        //public ICommand OrderByToggle { get; set; }

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
