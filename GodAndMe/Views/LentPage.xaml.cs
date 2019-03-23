using System;
using System.Linq;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LentPage : ContentPage
    {
        LentViewModel viewModel;

        public LentPage()
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
            BindingContext = viewModel = new LentViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Lent;
            if (item == null)
                return;

            await Navigation.PushAsync(new LentDetailPage(new LentDetailViewModel(item)));
            GetPageTitle();
            // Manually deselect item.
            LentListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LentPageNew(CommonFunctions.i18n("AddSavings")));
            GetPageTitle();
        }

        public void OnDuplicate(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            Lent oldItem = viewModel.Lent.First(x => x.Id == mi.CommandParameter.ToString());
            Lent newItem = new Lent()
            {
                Id = Guid.NewGuid().ToString(),
                Start = DateTime.Now,
                MoneyFrom = oldItem.MoneyFrom,
                MoneyTo = oldItem.MoneyTo,
                Text = oldItem.Text
            };
            MessagingCenter.Send(this, "AddItem", newItem);
            GetPageTitle();
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            var item = viewModel.Lent.First(x => x.Id == mi.CommandParameter.ToString()) as Lent;
            MessagingCenter.Send(this, "DeleteItem", item);
            viewModel.Lent.Remove(item);
            GetPageTitle();
        }

        private void GetPageTitle()
        {
            if (viewModel.Lent.Count > 0)
            {
                double SavedMoney = viewModel.Lent.Where((arg) => arg.Start.Year == DateTime.Today.Year).Sum((arg) => arg.SavedMoney);
                if (SavedMoney > 0)
                {
                    Title = string.Format(CommonFunctions.i18n("SavedXThisYear"), SavedMoney);
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Lent.Count == 0)
            {
                viewModel.LoadLentCommand.Execute(null);
            }
            GetPageTitle();
        }
    }
}