using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using GodAndMe.DependencyServices;
using GodAndMe.Views;
#if __IOS__
//using UIKit;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GodAndMe
{
    public partial class App : Application
    {
        static ITouchID touchId = DependencyService.Get<ITouchID>();
        // static IEnvironment environment = DependencyService.Get<IEnvironment>();
#if __IOS__
       // static UIView unlockView;
#endif
        public static bool justShowedUnlockView = false;
        public static bool justUnlocked = false;
        public const int DELAY = 123;
        //public static Theme theme = Theme.Light;
        public static bool unlocked_YN = false;

        public App()
        {

            //if (true)
            //{
            //    theme = environment.GetOperatingSystemTheme();
            //}
            InitializeComponent();
            //App.SetTheme();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
#endif
            Assembly assembly = typeof(App).GetTypeInfo().Assembly;
#if DEBUG
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            System.Diagnostics.Debug.WriteLine("====================================");
#endif
            // This lookup NOT required for Windows platforms - the Culture will be automatically set

#if __IOS__
            // determine the correct, supported .NET culture
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            GodAndMe.Resx.AppResources.Culture = ci; // set the RESX for resource localization
            DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
#elif __ANDROID__
            // determine the correct, supported .NET culture
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            GodAndMe.Resx.AppResources.Culture = ci; // set the RESX for resource localization
            DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
#endif

            if (touchId.CanAuthenticateUserIDWithTouchID())
            {
                Blur();
            }
            //GetTheme();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            justShowedUnlockView = false;
            justUnlocked = false;
            unlocked_YN = false;
            //GetTheme();
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (!justUnlocked && touchId.CanAuthenticateUserIDWithTouchID())
                {
                    unlocked_YN = await AuthenticatedWithTouchID();
                }
            });
        }

        protected override void OnSleep()
        {
            justShowedUnlockView = false;
            justUnlocked = false;
            unlocked_YN = false;

            // Handle when your app sleeps
            if (!justUnlocked && touchId.CanAuthenticateUserIDWithTouchID())
            {
                Blur();
            }
            unlocked_YN = false;
            justUnlocked = false;
        }

        protected override void OnResume()
        {
            justShowedUnlockView = false;
            justUnlocked = false;
            unlocked_YN = false;

            // Handle when your app resumes
            if (!unlocked_YN)
            {
                if (touchId.CanAuthenticateUserIDWithTouchID())
                {
                    Blur();
                }
                Device.BeginInvokeOnMainThread(async () =>
                {
                    unlocked_YN = await AuthenticatedWithTouchID();
                    if (unlocked_YN)
                    {
                        Sharpen();
                    }
                });
            }
            else
            {
                Sharpen();
            }
        }

        //public static void GetTheme()
        //{
        //    //theme = DependencyService.Get<IEnvironment>().GetOperatingSystemTheme();
        //    SetTheme();
        //}

        //public static void SetTheme()
        //{
        //    Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        //Handle Light Theme & Dark Theme
        //        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        //        if (mergedDictionaries != null)
        //        {
        //            mergedDictionaries.Clear();
        //            Uri url = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
        //            switch (theme)
        //            {
        //                case Theme.Dark:
        //                    mergedDictionaries.Add(new DarkTheme());
        //                    url = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
        //                    break;
        //                case Theme.Light:
        //                default:
        //                    mergedDictionaries.Add(new LightTheme());
        //                    break;
        //            }
        //            //Application.Current.Resources.Source = url;
        //        }
        //    });
        //}

        public static Task<bool> AuthenticatedWithTouchID()
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            if (!justUnlocked)
            {
                // Handle when your app starts
                Device.BeginInvokeOnMainThread(async () =>
                {
#if __IOS__
                    //UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                    //if (unlockView != null)
                    //{
                    //    foreach (UIView view in yourController.View.Subviews)
                    //    {
                    //        if (view.GetType() == typeof(UIVisualEffectView))
                    //        {
                    //            view.RemoveFromSuperview();
                    //        }
                    //    }
                    //    unlockView.RemoveFromSuperview();
                    //    Blur();
                    //}
#endif
                    bool _authenticatedWithTouchID = await touchId.AuthenticateUserIDWithTouchID();
                    if (_authenticatedWithTouchID)
                    {
                        Sharpen();
                        taskSource.SetResult(true);
                    }
                    else
                    {
                        Unlock();
                        taskSource.SetResult(false);
                    }
                });
            }
            else
            {
                taskSource.SetResult(true);
            }
            return taskSource.Task;
        }

        public static void Blur()
        {
            if (!justShowedUnlockView)
            {
#if __IOS__
                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                //    UIVisualEffect blurEffect = UIBlurEffect.FromStyle(Current.UserAppTheme == OSAppTheme.Light ? UIBlurEffectStyle.Light : UIBlurEffectStyle.Dark);
                //    UIVisualEffectView visualEffectView = new UIVisualEffectView(blurEffect)
                //    {
                //        Frame = yourController.View.Bounds,

                //    };
                //    yourController.View.AddSubview(visualEffectView);
                //    UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(visualEffectView);
                //});
#endif
            }
        }

        public static void Sharpen()
        {
            justShowedUnlockView = true;
#if __IOS__
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            //    foreach (UIView view in yourController.View.Subviews)
            //    {
            //        if (view.GetType() == typeof(UIVisualEffectView))
            //        {
            //            view.RemoveFromSuperview();
            //        }
            //    }
            //});
#endif
            Task.Factory.StartNew(() =>
            {
                Task.Delay(DELAY).Wait();
                justShowedUnlockView = false;
                justUnlocked = false;
            });
        }

        private static void Unlock()
        {
            justShowedUnlockView = true;
#if __IOS__
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            //    UIStoryboard storyboard = UIStoryboard.FromName("UnlockScreen", null);
            //    UIViewController viewController = storyboard.InstantiateViewController("UnlockScreen");
            //    unlockView = viewController.View;
            //    yourController.View.AddSubview(unlockView);
            //    UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(unlockView);
            //});
#endif
            Task.Factory.StartNew(() =>
            {
                Task.Delay(DELAY).Wait();
                justShowedUnlockView = false;
            });
        }
    }
}