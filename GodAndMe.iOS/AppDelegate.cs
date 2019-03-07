using System;
using System.Threading;
using Foundation;
using GodAndMe.iOS.Interface;
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
        public bool HasiCloud { get; private set; }
        public bool CheckingForiCloud { get; private set; }

        private NSUrl iCloudUrl;
        private NSError error;
        UIWindow window;
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();

            DependencyService.Register<FileStore>();
            DependencyService.Register<Share>();

            LoadApplication(new App());
            // GetUrlForUbiquityContainer is blocking, Apple recommends background thread or your UI will freeze
            ThreadPool.QueueUserWorkItem(_ =>
            {
                CheckingForiCloud = true;
                Console.WriteLine("Checking for iCloud");
                NSUrl uburl = NSFileManager.DefaultManager.GetUrlForUbiquityContainer(null);
                // OR instead of null you can specify "TEAMID.com.your-company.ApplicationName"

                if (uburl == null)
                {
                    HasiCloud = false;
                    Console.WriteLine("Can't find iCloud container, check your provisioning profile and entitlements");

#if DEBUG
                    InvokeOnMainThread(() =>
                    {
                        UIAlertController alertController = UIAlertController.Create("No \uE049 available", "Check your Entitlements.plist, BundleId, TeamId and Provisioning Profile!", UIAlertControllerStyle.Alert);
                        alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Destructive, null));
                        this.Window.RootViewController.PresentViewController(alertController, true, null);
                    });
#endif
                }
                else
                {
                    // iCloud enabled, store the NSURL for later use
                    HasiCloud = true;
                    iCloudUrl = uburl;
#if DEBUG
                    Console.WriteLine("yyy Yes iCloud! {0}", uburl);
#endif
                }
                CheckingForiCloud = false;
            });

            return base.FinishedLaunching(uiApplication, launchOptions);
        }
    }
}