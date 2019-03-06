using System;
using System.Collections.Generic;
using GodAndMe.Models;
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

        public IntentionPageNew(IntentionDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public IntentionPageNew(Intention intention = null)
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
                    Text = "",
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
                        List<string> persons = DependencyService.Get<IAddressBookInformation>().GetContacts();
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
            viewModel = new IntentionDetailViewModel(Intention);
            BindingContext = viewModel;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Intention = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Intention);
            await Navigation.PopModalAsync();
        }
    }
}