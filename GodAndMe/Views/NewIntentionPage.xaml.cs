using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GodAndMe.Models;
using GodAndMe.ViewModels;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewIntentionPage : ContentPage
    {
        IntentionsDetailViewModel viewModel;
        public Intention Intention { get; set; }

        public NewIntentionPage(IntentionsDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public NewIntentionPage(Intention Intention = null)
        {
            InitializeComponent();
            if (Intention != null)
            {
                Intention = Intention;
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

            viewModel = new IntentionsDetailViewModel(Intention);
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