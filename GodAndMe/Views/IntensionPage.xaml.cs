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
#if __IOS__
            var toolbarItem = new ToolbarItem();
            toolbarItem.IconImageSource = "hamburger.png";
            toolbarItem.Priority = -1;
            toolbarItem.Clicked += (object sender, EventArgs e) =>
            {
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = true;
            };
            ToolbarItems.Add(toolbarItem);
#endif

            BindingContext = viewModel = new IntentionViewModel();
            if (viewModel != null && viewModel.Items != null && viewModel.Items.Count == 0 && viewModel.LoadItemsCommand != null)
            {
                viewModel.LoadItemsCommand.Execute(null);
                GoToToday();
            }
        }

        void GoToToday()
        {
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

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args != null && args.SelectedItem != null && args.SelectedItem.GetType() == typeof(Intention))
            {
                if (!(args.SelectedItem is Intention item))
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
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Intention;
            item.Completed = !item.Completed;
            MessagingCenter.Send(this, "UpdateItem", item);
            viewModel.LoadItemsCommand.Execute(null);
            GoToToday();
        }

        public void OnShare(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Intention;
            try
            {
                string url = ("https://godandme.app/share/" + CryptFile.Encrypt_Legacy(JsonConvert.SerializeObject(new string[] { item.Person, item.Description, item.Start == null ? string.Empty : ((DateTime)item.Start).ToString("yyyy-MM-dd") })) + "?lang=" + CommonFunctions.Culture.TwoLetterISOLanguageName).Trim();
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
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString());
            if (item != null)
            {
                MessagingCenter.Send(this, "DeleteItem", item);
                if (viewModel.Items.Contains(item))
                {
                    viewModel.Items.Remove(item);
                }
            }
            GoToToday();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel != null && viewModel.Items != null && viewModel.Items.Count == 0 && viewModel.LoadItemsCommand != null)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}