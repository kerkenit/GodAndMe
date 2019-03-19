﻿using System;
using System.Globalization;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LentPageNew : ContentPage
    {
        LentDetailViewModel viewModel;
        public Lent Item { get; set; }

        public LentPageNew(string title, Lent lent = null)
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                ToolbarItem btCancel = new ToolbarItem()
                {
                    Text = CommonFunctions.i18n("Cancel"),
                    IsDestructive = true,
                    Priority = -1
                };

                btCancel.Clicked += async (object sender, EventArgs e) =>
                {
                    await Navigation.PopToRootAsync();
                };
                this.ToolbarItems.Add(btCancel);
            }

            if (lent != null)
            {
                Item = lent;
            }
            else
            {
                Item = new Lent
                {
                    Text = "",
                    MoneyFrom = null,
                    MoneyTo = 0,
                    Id = Guid.NewGuid().ToString(),
                    Start = DateTime.Now
                };
            }

            viewModel = new LentDetailViewModel(Item);
            viewModel.Title = title;
            BindingContext = viewModel;
            Device.BeginInvokeOnMainThread(() =>
           {
               tbMoneyFrom.Focus();
           });
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            viewModel.Item.MoneyFrom = double.Parse(tbMoneyFrom.Text, NumberStyles.Currency);
            viewModel.Item.MoneyTo = double.Parse(tbMoneyTo.Text, NumberStyles.Currency);
            Item = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}