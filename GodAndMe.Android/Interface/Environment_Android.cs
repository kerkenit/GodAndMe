using System;
using System.Threading.Tasks;
using Android.Content.Res;
using Plugin.CurrentActivity;
using Xamarin.Forms;
using GodAndMe;
using GodAndMe.Droid;
using Android.OS;

[assembly: Dependency(typeof(Environment_Android))]
namespace GodAndMe.Droid
{
    public class Environment_Android : IEnvironment
    {
        public Theme GetOperatingSystemTheme()
        {
            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
            if (false && Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
            {
                var uiModeFlags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

                switch (uiModeFlags)
                {
                    case UiMode.NightYes:
                        return Theme.Dark;

                    case UiMode.NightNo:
                        return Theme.Light;

                    default:
                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
                }
            }
            else
            {
                return Theme.Light;
            }
        }
    }
}
