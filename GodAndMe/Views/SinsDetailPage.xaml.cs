using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SinsDetailPage : ContentPage
    {
        SinsDetailViewModel viewModel;
        public Sins Item { get; set; }

        public SinsDetailPage(SinsDetailViewModel viewModel)
        {
            InitializeComponent();
            Title = string.Format("{0:D}", viewModel.Item.Start);
            BindingContext = this.viewModel = viewModel;
        }

        public SinsDetailPage()
        {
            InitializeComponent();

            Item = new Sins
            {
                Id = Guid.NewGuid().ToString(),
                Start = DateTime.Now
            };

            viewModel = new SinsDetailViewModel(Item);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.SinsDataStore != null))
            {
                Item = await viewModel.SinsDataStore.GetItemAsync(viewModel.Item.Id);
            }

            await Navigation.PushAsync(new SinsPageNew(Title, Item));
        }
    }
}