using System;
using GodAndMe.Models;
using GodAndMe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiaryPageNew : ContentPage
    {
        public DiaryDetailViewModel viewModel;
        public Diary Item { get; set; }

        public DiaryPageNew(string title, Diary item = null)
        {
            InitializeComponent();

            Title = title;
            if (item != null)
            {
                Item = item;
            }
            else
            {
                Item = new Diary
                {
                    Id = Guid.NewGuid().ToString(),
                    Start = DateTime.Now
                };
            }

            viewModel = new DiaryDetailViewModel(Item);
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