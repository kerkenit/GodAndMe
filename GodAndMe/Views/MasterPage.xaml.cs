using Xamarin.Forms;

namespace GodAndMe.Views
{
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.GetTheme();
        }
    }
}
