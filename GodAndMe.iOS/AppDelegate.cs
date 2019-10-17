using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using GodAndMe.iOS.Helpers;
using GodAndMe.iOS.Interface;
using GodAndMe.Views;
using UIKit;
using Xamarin.Forms;

namespace GodAndMe.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        const string URLSHEME = "godandme://";
        // class-level declarations
        NSObject observer;

        public override UIWindow Window { get; set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();

            Settings.LoadDefaultValues();
            observer = NSNotificationCenter.DefaultCenter.AddObserver((NSString)"NSUserDefaultsDidChangeNotification", DefaultsChanged);
            DefaultsChanged(null);

            DependencyService.Register<Share>();
            ///DependencyService.Register<TouchID>();


            LoadApplication(new App(theme));
            // GetUrlForUbiquityContainer is blocking, Apple recommends background thread or your UI will freeze


            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private Theme theme
        {
            get
            {
                //Ensure the current device is running 12.0 or higher, because `TraitCollection.UserInterfaceStyle` was introduced in iOS 12.0
                if (false && UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
                {
                    UIUserInterfaceStyle userInterfaceStyle = UIScreen.MainScreen.TraitCollection.UserInterfaceStyle;

                    switch (userInterfaceStyle)
                    {
                        case UIUserInterfaceStyle.Light:
                            return Theme.Light;
                        case UIUserInterfaceStyle.Dark:
                            return Theme.Dark;
                    }
                }
                return Theme.Light;
            }
        }

        /// <summary>
        /// Opens the URL.
        /// </summary>
        /// <returns><c>true</c>, if URL was opened, <c>false</c> otherwise.</returns>
        /// <param name="application">Application.</param>
        /// <param name="url">URL.</param>
        /// <param name="sourceApplication">Source application.</param>
        /// <param name="annotation">Annotation.</param>
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            string base64 = url.ToString();
            if ((Xamarin.Forms.Application.Current != null) && (Xamarin.Forms.Application.Current.MainPage != null))
            // custom stuff here using different properties of the url passed in
            {
                // custom stuff here using different properties of the url passed in
                ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenBase64(base64.Substring(CommonFunctions.URLSHEME.Length, base64.Length - CommonFunctions.URLSHEME.Length));
            }
            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            string dataToShare = null;
            if (!string.IsNullOrWhiteSpace(url.AbsoluteString))
            {
                dataToShare = url.AbsoluteString;
            }
            if (string.IsNullOrWhiteSpace(dataToShare) && !string.IsNullOrWhiteSpace(url.Host))
            {
                dataToShare = url.Host;
            }

            if (dataToShare.StartsWith(CommonFunctions.URLSHEME, StringComparison.Ordinal))
            {
                ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenBase64(dataToShare.Substring(CommonFunctions.URLSHEME.Length, dataToShare.Length - CommonFunctions.URLSHEME.Length));
            }
            else
            {
#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                {
                    string fname = url.Path;
                    using (Stream stm = new FileStream(fname, FileMode.Open, FileAccess.Read))
                    {
                        using (StreamReader rdr = new StreamReader(stm))
                        {
                            dataToShare = rdr.ReadToEnd();
                        }
                    }
                }
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            }
            if (dataToShare != null)
            {
                ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenJson(dataToShare);
            }
            return true;
        }


        /// <summary>
        /// This method is called when the application is about to terminate. Save data, if needed.
        /// </summary>
        /// <seealso cref="DidEnterBackground"/>
        public override void WillTerminate(UIApplication uiApplication)
        {
            if (observer != null)
            {
                NSNotificationCenter.DefaultCenter.RemoveObserver(observer);
                observer = null;
            }
        }

        void DefaultsChanged(NSNotification obj)
        {
            Settings.SetupByPreferences();
        }

        /*
        public override void WillEnterForeground(UIApplication application)
        {
            if (!Settings.TouchID)
            {
                return;
            }

            LocalAuthHelper.Authenticate(null, // do not do anything on success
            () =>
            {
                // show View Controller that requires authentication
                InvokeOnMainThread(() =>
                        {
                            var localAuthViewController = new AuthenticationViewController();
                            Window.RootViewController.ShowViewController(localAuthViewController, null);
                        });
            });
        }
        */
    }
}