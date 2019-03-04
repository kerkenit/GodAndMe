using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GodAndMe.Models;
using GodAndMe.ViewModels;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionDetailPage : ContentPage
    {
        IntentionsDetailViewModel viewModel;
        public Intention Intention { get; set; }

        public IntentionDetailPage(IntentionsDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public IntentionDetailPage()
        {
            InitializeComponent();

            Intention = new Intention
            {
                Text = "",
                Description = "",
                Id = Guid.NewGuid().ToString(),
                Start = null
            };

            viewModel = new IntentionsDetailViewModel(Intention);
            BindingContext = viewModel;
        }

        async void EditItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewIntentionPage(this.viewModel.Item)));

        }
    }
}