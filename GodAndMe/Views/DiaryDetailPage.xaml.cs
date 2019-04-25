using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryDetailPage : ContentPage
    {
        DiaryDetailViewModel viewModel;
        public Diary Item { get; set; }

        public DiaryDetailPage(DiaryDetailViewModel viewModel)
        {
            InitializeComponent();
            Title = CommonFunctions.Date(viewModel.Item.Start);
            BindingContext = this.viewModel = viewModel;
        }

        public DiaryDetailPage()
        {
            InitializeComponent();

            Item = new Diary
            {
                Id = Guid.NewGuid().ToString(),
                Start = DateTime.Now
            };

            viewModel = new DiaryDetailViewModel(Item);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.DiaryDataStore != null))
            {
                Item = await viewModel.DiaryDataStore.GetItemAsync(viewModel.Item.Id);
            }

            await Navigation.PushAsync(new DiaryPageNew(Title, Item));
        }
    }
}