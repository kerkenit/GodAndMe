using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionDetailPage : ContentPage
    {
        IntentionDetailViewModel viewModel;
        public Intention Item { get; set; }

        public IntentionDetailPage(IntentionDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public IntentionDetailPage()
        {
            InitializeComponent();

            Item = new Intention
            {
                //Text = "",
                Description = "",
                Id = Guid.NewGuid().ToString(),
                Start = null
            };

            viewModel = new IntentionDetailViewModel(Item);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.IntentionDataStore != null))
            {
                Item = await viewModel.IntentionDataStore.GetItemAsync(viewModel.Item.Id);
            }

            await Navigation.PushAsync(new IntentionPageNew(CommonFunctions.i18n("EditIntention"), Item));
        }
    }
}