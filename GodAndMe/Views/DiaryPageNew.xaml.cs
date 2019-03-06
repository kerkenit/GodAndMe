using System;
using GodAndMe.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPageNew : ContentPage
    {
        public Item Item { get; set; }

        public DiaryPageNew()
        {
            InitializeComponent();

            Item = new Item
            {
                Text = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }
    }
}