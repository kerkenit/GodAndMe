using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace GodAndMe.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Resx.TranslateExtension a = new Resx.TranslateExtension();

            Title = a.GetValue("About");

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://www.godandme.app")));
        }

        public ICommand OpenWebCommand { get; }
    }
}