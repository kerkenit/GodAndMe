using System;
using System.Linq;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPage : ContentPage
    {
        DiaryViewModel viewModel;

        public DiaryPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DiaryViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Diary;
            if (item == null)
                return;

            await Navigation.PushAsync(new DiaryDetailPage(new DiaryDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DiaryPageNew(CommonFunctions.i18n("NewIntention")));
        }

        public void OnDelete(object sender, EventArgs e)
        {
            //viewModel.IsBusy = true;

            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Diary;
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