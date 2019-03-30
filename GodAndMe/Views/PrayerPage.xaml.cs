using System;
using System.Linq;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayersPage : ContentPage
    {
        PrayersViewModel viewModel;

        public PrayersPage()
        {
            InitializeComponent();
#if __IOS__
            var toolbarItem = new ToolbarItem();
            toolbarItem.Icon = "hamburger.png";
            toolbarItem.Priority = -1;
            toolbarItem.Clicked += (object sender, EventArgs e) =>
            {
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = true;
            };
            ToolbarItems.Add(toolbarItem);
#endif
            BindingContext = viewModel = new PrayersViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Prayers;
            if (item == null)
                return;

            await Navigation.PushAsync(new PrayersDetailPage(new PrayersDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PrayersPageNew(CommonFunctions.i18n("NewPrayer")));
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString());
            if (item != null)
            {
                MessagingCenter.Send(this, "DeleteItem", item);
                if (viewModel.Items.Contains(item))
                {
                    viewModel.Items.Remove(item);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}