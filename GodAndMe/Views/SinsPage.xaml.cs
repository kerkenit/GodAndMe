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
    public partial class SinsPage : ContentPage
    {
        SinsViewModel viewModel;

        public SinsPage()
        {
            InitializeComponent();

            App.justUnlocked = true;
            App.justShowedUnlockView = false;
            App.AuthenticatedWithTouchID();

#if __IOS__
            var toolbarItem = new ToolbarItem();
            toolbarItem.Icon = "hamburger.png";
            toolbarItem.Priority = -1;
            toolbarItem.Clicked += (object sender, EventArgs e) =>
            {
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = true;
            };
            ToolbarItems.Add(toolbarItem);
#endif
            BindingContext = viewModel = new SinsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (!(args.SelectedItem is Sins item))
                return;

            await Navigation.PushAsync(new SinsDetailPage(new SinsDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SinsPageNew(CommonFunctions.i18n("NewSin")));
        }

        public void OnArchive(object sender, EventArgs e)
        {
            //viewModel.IsBusy = true;
            var mi = ((MenuItem)sender);
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Sins;
            item.Confessed = true;
            MessagingCenter.Send(this, "UpdateItem", item);
            viewModel.LoadItemsCommand.Execute(null);
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
            var item = viewModel.Items.First(x => x.Id == mi.CommandParameter.ToString()) as Sins;
            try
            {
                string url = (CommonFunctions.URLSHEME + CryptFile.Encrypt(JsonConvert.SerializeObject(item))).Trim();
                var share = DependencyService.Get<IShare>();
                share.Show(
                    CommonFunctions.i18n("Sin"),
                    item.Committed.ToString("D"),
                    url,
                    ""
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}