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

#if __IOS__
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
#endif

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
                    Committed = DateTime.Now,
                    Description = string.Empty,

                };
            }

            viewModel = new SinsDetailViewModel(Item);
            BindingContext = viewModel;

            Device.BeginInvokeOnMainThread(() =>
            {
                tbDescription.Focus();
            });
        }

        void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                tbDescription.Focus();
            });
        }

        void OnTextChanged(Object sender, TextChangedEventArgs e)
        {
            tbDescription.InvalidateLayout();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            viewModel.Item.Description = viewModel.Item.Description.Trim().Trim(Environment.NewLine.ToCharArray()).Trim();
            Item = viewModel.Item;
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}