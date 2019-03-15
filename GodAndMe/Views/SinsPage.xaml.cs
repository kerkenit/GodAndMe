using System;
using System.Linq;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SinsPage : ContentPage
    {
        SinsViewModel viewModel;

        public SinsPage()
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
            }
            BindingContext = viewModel = new SinsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Sins;
            if (item == null)
                return;

            await Navigation.PushAsync(new SinsDetailPage(new SinsDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SinsPageNew(CommonFunctions.i18n("NewIntention")));
        }

        public void OnDelete(object sender, EventArgs e)
        {
            //viewModel.IsBusy = true;

            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Sins;
            MessagingCenter.Send(this, "DeleteItem", item);
            viewModel.Items.Remove(item);

            //viewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}