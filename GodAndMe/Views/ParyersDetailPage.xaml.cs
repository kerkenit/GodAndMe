using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrayersDetailPage : ContentPage
    {
        PrayersDetailViewModel viewModel;
        public Prayers Item { get; set; }

        public PrayersDetailPage(PrayersDetailViewModel viewModel)
        {
            InitializeComponent();
            Title = string.Format("{0:D}", viewModel.Item.Title);
            BindingContext = this.viewModel = viewModel;
        }

        public PrayersDetailPage()
        {
            InitializeComponent();

            Item = new Prayers
            {
                Id = Guid.NewGuid().ToString(),
            };

            viewModel = new PrayersDetailViewModel(Item);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.PrayersDataStore != null))
            {
                Item = await viewModel.PrayersDataStore.GetItemAsync(viewModel.Item.Id);
            }

            await Navigation.PushAsync(new PrayersPageNew(Title, Item));
        }
    }
}