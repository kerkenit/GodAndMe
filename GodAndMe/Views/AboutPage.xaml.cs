using System;
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
#if __IOS__
            var toolbarItem = new ToolbarItem();
            toolbarItem.IconImageSource = "hamburger.png";
            toolbarItem.Priority = -1;
            toolbarItem.Clicked += (object sender, EventArgs e) =>
            {
                ((MasterDetailPage)Application.Current.MainPage).IsPresented = true;
            };
            ToolbarItems.Add(toolbarItem);
#endif
            lblVersionNumber.Text = DependencyService.Get<IAppVersionAndBuild>().GetVersionNumber();
        }
    }
}