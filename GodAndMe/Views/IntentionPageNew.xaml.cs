﻿using System;
using System.Collections.Generic;
using GodAndMe.Models;
using GodAndMe.Services;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntentionPageNew : ContentPage
    {
        public IntentionDetailViewModel viewModel;
        public Intention Item { get; set; }

        //public IntentionPageNew(IntentionDetailViewModel viewModel, string title)
        //{
        //    InitializeComponent();
        //    viewModel.Title = title;
        //    BindingContext = this.viewModel = viewModel;
        //}

        public IntentionPageNew(string title, Intention item = null)
        {
            InitializeComponent();

            if (item != null)
            {
                Item = item;
            }
            else
            {
                Item = new Intention
                {
                    //Text = "",
                    Description = "",
                    Id = Guid.NewGuid().ToString(),
                    Start = null
                };
            }

            btPerson.Clicked += async (object sender, EventArgs e) =>
            {
                string ChooseOther = "- " + CommonFunctions.i18n("ChooseOther") + " -";
                string ChooseRecent = "- " + CommonFunctions.i18n("ChooseRecent") + " -";


                if ((ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0) || await DependencyService.Get<IAddressBookInformation>().RequestAccess())
                {
                    if (ddlPerson.ItemsSource == null || ddlPerson.ItemsSource.Count == 0)
                    {
                        List<string> persons = await DependencyService.Get<IAddressBookInformation>().GetContacts();

                        IEnumerable<Intention> existingItems = await viewModel.IntentionDataStore.GetItemsAsync(true);
                        var recentPersons =
    from intension in existingItems
    group intension by intension.Person into Intensions
    select new
    {
        Name = Intensions.Key,
        Count = Intensions.Count(),
    };
                        persons.Insert(0, ChooseOther);


                        foreach (var person in recentPersons.OrderBy((arg) => arg.Count))
                        {
                            persons.Remove(person.Name);
                            persons.Insert(0, person.Name);
                        }
                        if (recentPersons.Any())
                        {
                            persons.Insert(0, ChooseRecent);
                        }
                        ddlPerson.ItemsSource = persons;
                        if (!string.IsNullOrWhiteSpace(tbPerson.Text))
                        {
                            ddlPerson.SelectedItem = tbPerson.Text;
                        }
                    }
                    if (ddlPerson.ItemsSource != null && ddlPerson.ItemsSource.Count > 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            ddlPerson.Unfocused += (object sender1, FocusEventArgs e1) =>
                            {
                                if (ddlPerson.SelectedItem != null && ddlPerson.SelectedItem.ToString() != ChooseOther && ddlPerson.SelectedItem.ToString() != ChooseRecent)
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

            ddlStart.Unfocused += (object sender, FocusEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    tbStart.Text = string.Format("{0:D}", ddlStart.Date);
                    btStart.IsEnabled = true;
                });
            };

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
                Item.Start = null;
                Device.BeginInvokeOnMainThread(() =>
                {
                    btStart.IsEnabled = false;
                    tbStart.Text = string.Empty;
                });
            };
            tbStart.Focused += (object sender, FocusEventArgs e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ddlStart.Focus();
                });
            };
            btStart.IsEnabled = Item.Start != null;
            viewModel = new IntentionDetailViewModel(Item);
            viewModel.Title = title;
            BindingContext = viewModel;
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            Item = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}