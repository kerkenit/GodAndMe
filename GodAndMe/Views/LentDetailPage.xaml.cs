using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LentDetailPage : ContentPage
    {
        LentDetailViewModel viewModel;
        public Lent Lent { get; set; }

        public LentDetailPage(LentDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public LentDetailPage()
        {
            InitializeComponent();

            Lent = new Lent
            {
                Text = "",
                MoneyFrom = null,
                MoneyTo = 0,
                Id = Guid.NewGuid().ToString(),
                Start = DateTime.Now
            };

            viewModel = new LentDetailViewModel(Lent);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            if ((viewModel != null) && (viewModel.Item != null) && (viewModel.LentDataStore != null))
            {
                Lent = await viewModel.LentDataStore.GetItemAsync(viewModel.Item.Id);
            }

            await Navigation.PushAsync(new LentPageNew(CommonFunctions.i18n("EditSavings"), Lent));
        }
    }
}