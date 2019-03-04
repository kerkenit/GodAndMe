using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GodAndMe.Models;
using GodAndMe.Views;
using GodAndMe.ViewModels;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionsPage : ContentPage
    {
        IntentionsViewModel viewModel;

        public IntentionsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new IntentionsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Intention;
            if (item == null)
                return;

            await Navigation.PushAsync(new IntentionDetailPage(new IntentionsDetailViewModel(item)));

            // Manually deselect item.
            IntentionsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewIntentionPage()));
        }

        public void OnMore(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = viewModel.Intentions.Select(x => x.Id == mi.CommandParameter.ToString()) as Intention;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            var mi = ((MenuItem)sender);
            var item = viewModel.Intentions.First(x => x.Id == mi.CommandParameter.ToString()) as Intention;
            MessagingCenter.Send(this, "DeleteItem", item);
            viewModel.Intentions.Remove(item);

            viewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Intentions.Count == 0)
                viewModel.LoadIntentionsCommand.Execute(null);
        }
    }
}