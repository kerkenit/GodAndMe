﻿using System;
using System.IO;
using System.Threading;
using Foundation;
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

        const string URLSHEME = "GodAndMe://";
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

            LoadApplication(new App());
            // GetUrlForUbiquityContainer is blocking, Apple recommends background thread or your UI will freeze


            return base.FinishedLaunching(uiApplication, launchOptions);
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
            string base64 = url.ToString().Substring(URLSHEME.Length, url.ToString().Length - URLSHEME.Length);
            if ((Xamarin.Forms.Application.Current != null) && (Xamarin.Forms.Application.Current.MainPage != null))
            // custom stuff here using different properties of the url passed in
            {
                // custom stuff here using different properties of the url passed in
                ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenBase64(base64);
            }
            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            if (!string.IsNullOrWhiteSpace(url.Path) && UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {

#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
                string dataToShare = File.ReadAllText(url.AbsoluteString);
                if (dataToShare != null)
                {
                    ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenJson(dataToShare.ToString());
                }
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
            }
            else if (url.Host != null)
            {
                string dataToShare = url.Host;
                if (dataToShare != null)
                {
                    ((MainPage)Xamarin.Forms.Application.Current.MainPage).OpenBase64(dataToShare.ToString());
                }
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
    }
}