using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using GodAndMe.DependencyServices;
using GodAndMe.Views;
using MvvmCross.Platform;
using Plugin.Fingerprint.Abstractions;
#if __iOS__
using UIKit;
#endif
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GodAndMe
{
    public partial class App : Application
    {
        static ITouchID touchId = DependencyService.Get<ITouchID>();
#if __iOS__
        static UIView unlockView;
#endif
        bool appLocked = false;
        public static bool justShowedUnlockView = false;
        public static bool justUnlocked = false;
        public const int DELAY = 780;
        public App()
        {
            InitializeComponent();

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
            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                // determine the correct, supported .NET culture
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                GodAndMe.Resx.AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            if (touchId.CanAuthenticateUserIDWithTouchID())
            {
                Blur();
            }

            MainPage = new MainPage();
        }



        protected override void OnStart()
        {
            justUnlocked = true;

            Device.BeginInvokeOnMainThread(async () =>
            {
                appLocked = await AuthenticatedWithTouchID();
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            appLocked = true;
            justUnlocked = true;
            if (touchId.CanAuthenticateUserIDWithTouchID())
            {
                Blur();
            }
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            if (appLocked)
            {
                if (touchId.CanAuthenticateUserIDWithTouchID())
                {
                    Blur();
                }
                Device.BeginInvokeOnMainThread(async () =>
                {
                    appLocked = await AuthenticatedWithTouchID();
                });
            }
        }

        public static Task<bool> AuthenticatedWithTouchID()
        {

            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            if (!justShowedUnlockView)
            {
                // Handle when your app starts
                Device.BeginInvokeOnMainThread(async () =>
                {
#if __iOS__
                    UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                    if (unlockView != null)
                    {
                        foreach (UIView view in yourController.View.Subviews)
                        {
                            if (view.GetType() == typeof(UIVisualEffectView))
                            {
                                view.RemoveFromSuperview();
                            }
                        }
                        unlockView.RemoveFromSuperview();
                        Blur();
                    }
#endif
                    IFingerprint fingerPrintService = Mvx.Resolve<IFingerprint>(); // or use dependency injection and inject IFingerprint

                    FingerprintAuthenticationResult result = await fingerPrintService.AuthenticateAsync(CommonFunctions.i18n("UnlockToOpenGodAndMe"));
                    if (result.Authenticated)
                    {
                        Sharpen();
                        taskSource.SetResult(false);
                    }
                    //else
                    //{
                    //    Unlock();
                    //    taskSource.SetResult(true);
                    //}
                    //bool _authenticatedWithTouchID = await touchId.AuthenticateUserIDWithTouchID();
                    //if (_authenticatedWithTouchID)
                    //{
                    //    Sharpen();
                    //    taskSource.SetResult(false);
                    //}
                    //else
                    //{
                    //    Unlock();
                    //    taskSource.SetResult(true);
                    //}
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
#if __iOS__
                Device.BeginInvokeOnMainThread(() =>
                {
                    UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                    UIVisualEffect blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
                    UIVisualEffectView visualEffectView = new UIVisualEffectView(blurEffect)
                    {
                        Frame = yourController.View.Bounds,

                    };
                    yourController.View.AddSubview(visualEffectView);
                    UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(visualEffectView);
                });
#endif
            }
        }

        public static void Sharpen()
        {
            justShowedUnlockView = true;
#if __iOS__
            Device.BeginInvokeOnMainThread(() =>
            {
                UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;

                foreach (UIView view in yourController.View.Subviews)
                {
                    if (view.GetType() == typeof(UIVisualEffectView))
                    {
                        view.RemoveFromSuperview();
                    }
                }
            });
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
#if __iOS__
            Device.BeginInvokeOnMainThread(() =>
            {
                UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                UIStoryboard storyboard = UIStoryboard.FromName("UnlockScreen", null);
                UIViewController viewController = storyboard.InstantiateViewController("UnlockScreen");
                unlockView = viewController.View;
                yourController.View.AddSubview(unlockView);
                UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(unlockView);
            });
#endif
            Task.Factory.StartNew(() =>
            {
                Task.Delay(DELAY).Wait();
                justShowedUnlockView = false;
            });
        }
    }
}