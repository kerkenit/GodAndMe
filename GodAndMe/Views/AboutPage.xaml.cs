using GodAndMe.DependencyServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GodAndMe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            lblVersionNumber.Text = DependencyService.Get<IAppVersionAndBuild>().GetVersionNumber();
        }
    }
}