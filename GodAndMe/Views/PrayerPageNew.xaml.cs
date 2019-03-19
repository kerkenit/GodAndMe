using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayersPageNew : ContentPage
    {
        public PrayersDetailViewModel viewModel;
        public Prayers Item { get; set; }

        public PrayersPageNew(string title, Prayers item = null)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                ToolbarItem btCancel = new ToolbarItem()
                {
                    Text = CommonFunctions.i18n("Cancel"),
                    IsDestructive = true,
                    Priority = -1
                };

                btCancel.Clicked += async (object sender, EventArgs e) =>
                {
                    await Navigation.PopToRootAsync();
                };
                this.ToolbarItems.Add(btCancel);
            }

            Title = title;
            if (item != null)
            {
                Item = item;
            }
            else
            {
                Item = new Prayers
                {
                    Id = Guid.NewGuid().ToString(),
                };
            }

            viewModel = new PrayersDetailViewModel(Item);
            BindingContext = viewModel;
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
           {
               tbDescription.Focus();
           });
        }

        void OnTextChanged(Object sender, TextChangedEventArgs e)
        {
            tbDescription.InvalidateLayout();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}