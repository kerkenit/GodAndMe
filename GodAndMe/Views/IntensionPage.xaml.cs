using System;
using System.Linq;
using GodAndMe.Extensions;
using GodAndMe.Interface;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionPage : ContentPage
    {
        public IntentionViewModel viewModel;

        public IntentionPage()
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

            BindingContext = viewModel = new IntentionViewModel();
            if (viewModel != null && viewModel.Items != null && viewModel.Items.Count == 0 && viewModel.LoadIntentionsCommand != null)
            {
                viewModel.LoadIntentionsCommand.Execute(null);
                if (IntentionsListView != null && IntentionsListView.ItemsSource != null && viewModel != null && viewModel.Items != null && viewModel.Items.Any((arg) => !arg.Completed))
                {
                    Intention firstIntension = IntentionsListView.ItemsSource.Cast<Intention>().OrderBy((arg) => arg.Completed ? DateTime.MinValue : arg.Start == null ? DateTime.Today : arg.Start).FirstOrDefault((arg) => !arg.Completed);
                    if (firstIntension != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            if (IntentionsListView != null)
                            {
                                IntentionsListView.ScrollTo(firstIntension, ScrollToPosition.Start, false);
                            }
                        });
                    }
                }
            }
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args != null && args.SelectedItem != null && args.SelectedItem.GetType() == typeof(Intention))
            {
                Intention item = args.SelectedItem as Intention;
                if (item == null)
                    return;

                await Navigation.PushAsync(new IntentionDetailPage(new IntentionDetailViewModel(item)));

                // Manually deselect item.
                if (IntentionsListView != null)
                {
                    try
                    {
                        IntentionsListView.SelectedItem = null;
                    }
                    finally
                    {

                    }
                }
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new IntentionPageNew(CommonFunctions.i18n("NewIntention")));
        }

        public void OnArchive(object sender, EventArgs e)
        {
            //viewModel.IsBusy = true;
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Intention;
            item.Completed = !item.Completed;
            MessagingCenter.Send(this, "UpdateItem", item);
            viewModel.LoadIntentionsCommand.Execute(null);
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    IntentionsListView.Unfocus();
            //});
            //viewModel.Intentions.Remove(item);
            //viewModel.LoadIntentionsCommand.Execute(null);
            //viewModel.IsBusy = false;
        }

        public void OnShare(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Intention;
            try
            {
                string url = (CommonFunctions.URLSHEME + StringExtensions.Base64Encode(JsonConvert.SerializeObject(item))).Trim();
                var share = DependencyService.Get<IShare>();
                share.Show(
                    string.Format(CommonFunctions.i18n("WouldYouPrayForX"), item.Person),
                    item.Description,
                    url,
                    Environment.NewLine + Environment.NewLine + CommonFunctions.i18n("DownloadApp")
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        public void OnDelete(object sender, EventArgs e)
        {
            //viewModel.IsBusy = true;

            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Intention;
            MessagingCenter.Send(this, "DeleteItem", item);
            viewModel.Items.Remove(item);

            //viewModel.IsBusy = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel != null && viewModel.Items != null && viewModel.Items.Count == 0 && viewModel.LoadIntentionsCommand != null)
            {
                viewModel.LoadIntentionsCommand.Execute(null);
            }
        }
    }
}