using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GodAndMe.DependencyServices;

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