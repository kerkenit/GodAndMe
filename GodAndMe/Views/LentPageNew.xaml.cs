using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using GodAndMe.Models;
using GodAndMe.Services;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LentPageNew : ContentPage
    {
        LentDetailViewModel viewModel;
        public Lent Lent { get; set; }

        //public LentPageNew(LentDetailViewModel viewModel, string title)
        //{
        //    InitializeComponent();
        //    viewModel.Title = title;
        //    BindingContext = this.viewModel = viewModel;
        //}

        public LentPageNew(string title, Lent lent = null)
        {
            InitializeComponent();


            if (lent != null)
            {
                Lent = lent;
            }
            else
            {
                Lent = new Lent
                {
                    Text = "",
                    MoneyFrom = null,
                    MoneyTo = 0,
                    Id = Guid.NewGuid().ToString(),
                    Start = DateTime.Now
                };
            }

            //btPerson.Clicked += async (object sender, EventArgs e) =>
            //{
            //    if ((ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0) || await DependencyService.Get<IAddressBookInformation>().RequestAccess())
            //    {
            //        if (ddlPerson.ItemsSource == null || ddlPerson.ItemsSource.Count == 0)
            //        {
            //            List<string> persons = await DependencyService.Get<IAddressBookInformation>().GetContacts();
            //            persons.Insert(0, "- " + CommonFunctions.i18n("ChooseOther") + " -");
            //            ddlPerson.ItemsSource = persons;
            //        }
            //        if (ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0)
            //        {
            //            Device.BeginInvokeOnMainThread(() =>
            //            {
            //                ddlPerson.Unfocused += (object sender1, FocusEventArgs e1) =>
            //                {
            //                    if (ddlPerson.SelectedItem != null && ddlPerson.SelectedIndex > 0)
            //                    {
            //                        tbPerson.Text = ddlPerson.SelectedItem.ToString();
            //                    }
            //                    else
            //                    {
            //                        tbPerson.Text = string.Empty;
            //                        ddlPerson.Unfocus();
            //                    }
            //                };
            //                ddlPerson.Focus();
            //            });
            //        }
            //    }
            //};

            ////ddlStart.Unfocused += (object sender, FocusEventArgs e) =>
            ////{
            ////    tbStart.Text = string.Empty;
            ////};

            //ddlStart.DateSelected += (object sender, DateChangedEventArgs e) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        tbStart.Text = string.Format("{0:D}", ddlStart.Date);
            //        btStart.IsEnabled = true;
            //    });
            //};
            //btStart.Clicked += (object sender, EventArgs e) =>
            //{
            //    btStart.IsEnabled = false;
            //    Lent.Start = null;
            //    tbStart.Text = string.Empty;
            //};
            //tbStart.Focused += (object sender, FocusEventArgs e) =>
            //{
            //    Device.BeginInvokeOnMainThread(() =>
            //    {
            //        ddlStart.Focus();
            //    });
            //};
            //btStart.IsEnabled = Lent.Start != null;
            viewModel = new LentDetailViewModel(Lent);
            viewModel.Title = title;
            BindingContext = viewModel;
            Device.BeginInvokeOnMainThread(async () =>
            {
                tbMoneyFrom.Focus();
            });
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            //if ((viewModel != null) && (viewModel.Item != null))
            //{
            //    Lent = await viewModel.LentDataStore.GetItemAsync(Lent.Id);
            //    viewModel.Item = Lent;
            //    BindingContext = viewModel.Item;
            //}
            //MessagingCenter.Send(this, "GetItem", Lent);
            await Navigation.PopAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            viewModel.Item.MoneyFrom = double.Parse(tbMoneyFrom.Text, NumberStyles.Currency);
            viewModel.Item.MoneyTo = double.Parse(tbMoneyTo.Text, NumberStyles.Currency);
            Lent = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Lent);
            await Navigation.PopAsync();
        }
    }
}