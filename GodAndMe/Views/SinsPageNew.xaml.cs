using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SinsPageNew : ContentPage
    {
        public SinsDetailViewModel viewModel;
        public Sins Item { get; set; }

        public SinsPageNew(string title, Sins item = null)
        {
            InitializeComponent();

            Title = title;
            if (item != null)
            {
                Item = item;
            }
            else
            {
                Item = new Sins
                {
                    Id = Guid.NewGuid().ToString(),
                    Start = DateTime.Now
                };
            }

            viewModel = new SinsDetailViewModel(Item);
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