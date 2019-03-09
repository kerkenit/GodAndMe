using System;
using System.Collections.Generic;
using GodAndMe.Models;
using GodAndMe.Services;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionPageNew : ContentPage
    {
        IntentionDetailViewModel viewModel;
        public Intention Intention { get; set; }

        //public IntentionPageNew(IntentionDetailViewModel viewModel, string title)
        //{
        //    InitializeComponent();
        //    viewModel.Title = title;
        //    BindingContext = this.viewModel = viewModel;
        //}

        public IntentionPageNew(string title, Intention intention = null)
        {
            InitializeComponent();

            if (intention != null)
            {
                Intention = intention;
            }
            else
            {
                Intention = new Intention
                {
                    //Text = "",
                    Description = "",
                    Id = Guid.NewGuid().ToString(),
                    Start = null
                };
            }

            btPerson.Clicked += async (object sender, EventArgs e) =>
            {
                if ((ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0) || await DependencyService.Get<IAddressBookInformation>().RequestAccess())
                {
                    if (ddlPerson.ItemsSource == null || ddlPerson.ItemsSource.Count == 0)
                    {
                        List<string> persons = await DependencyService.Get<IAddressBookInformation>().GetContacts();
                        persons.Insert(0, "- " + CommonFunctions.i18n("ChooseOther") + " -");
                        ddlPerson.ItemsSource = persons;
                    }
                    if (ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ddlPerson.Unfocused += (object sender1, FocusEventArgs e1) =>
                            {
                                if (ddlPerson.SelectedItem != null && ddlPerson.SelectedIndex > 0)
                                {
                                    tbPerson.Text = ddlPerson.SelectedItem.ToString();
                                }
                                else
                                {
                                    tbPerson.Text = string.Empty;
                                    ddlPerson.Unfocus();
                                }
                            };
                            ddlPerson.Focus();
                        });
                    }
                }
            };

            //ddlStart.Unfocused += (object sender, FocusEventArgs e) =>
            //{
            //    tbStart.Text = string.Empty;
            //};

            ddlStart.DateSelected += (object sender, DateChangedEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    tbStart.Text = string.Format("{0:D}", ddlStart.Date);
                    btStart.IsEnabled = true;
                });
            };
            btStart.Clicked += (object sender, EventArgs e) =>
            {
                btStart.IsEnabled = false;
                Intention.Start = null;
                tbStart.Text = string.Empty;
            };
            tbStart.Focused += (object sender, FocusEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ddlStart.Focus();
                });
            };
            btStart.IsEnabled = Intention.Start != null;
            viewModel = new IntentionDetailViewModel(Intention);
            viewModel.Title = title;
            BindingContext = viewModel;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            //if ((viewModel != null) && (viewModel.Item != null))
            //{
            //    Intention = await viewModel.IntentionDataStore.GetItemAsync(Intention.Id);
            //    viewModel.Item = Intention;
            //    BindingContext = viewModel.Item;
            //}
            //MessagingCenter.Send(this, "GetItem", Intention);
            await Navigation.PopAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Intention = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Intention);
            await Navigation.PopAsync();
        }
    }
}