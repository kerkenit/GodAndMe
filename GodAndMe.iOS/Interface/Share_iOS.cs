using System;
using System.Threading.Tasks;
using Foundation;
using GodAndMe.Extensions;
using GodAndMe.Interface;
using PCLStorage;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.iOS.Interface.Share))]

namespace GodAndMe.iOS.Interface
{
    public class Share : IShare
    {
        // MUST BE CALLED FROM THE UI THREAD
        public async Task Show(string title, string message, string url, string app)
        {
            NSObject[] items = null;
            if (message != null && app != null)
            {
                items = new NSObject[] { NSObject.FromObject(title + Environment.NewLine + message + Environment.NewLine + Environment.NewLine + url + app) };
            }
            else
            {

                //IFileSystem fileSystem = FileSystem.Current;

                //// Get the root directory of the file system for our application.
                //IFolder rootFolder = fileSystem.LocalStorage;
                //IFile json = await fileSystem.GetFileFromPathAsync(url);
                //string text = await json.ReadAllTextAsync();


                items = new NSObject[] { NSUrl.FromFilename(url) };

                if (items == null)
                {
                    if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    {
                        using (NSData dataToShare = NSFileManager.DefaultManager.Contents(url))
                        {
                            items = new NSObject[] { dataToShare };
                        }
                    }
                }

            }
            UIActivityViewController activityController = new UIActivityViewController(items, null);
            UIViewController vc = GetVisibleViewController();

            NSString[] excludedActivityTypes = null;

            if (excludedActivityTypes != null && excludedActivityTypes.Length > 0)
                activityController.ExcludedActivityTypes = excludedActivityTypes;

            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                if (activityController.PopoverPresentationController != null)
                {
                    activityController.PopoverPresentationController.SourceView = vc.View;
                }
            }
            await vc.PresentViewControllerAsync(activityController, true);
        }

        UIViewController GetVisibleViewController()
        {
            UIViewController rootController = UIApplication.SharedApplication.KeyWindow.RootViewController;

            if (rootController.PresentedViewController == null)
                return rootController;

            if (rootController.PresentedViewController is UINavigationController)
            {
                return ((UINavigationController)rootController.PresentedViewController).TopViewController;
            }

            if (rootController.PresentedViewController is UITabBarController)
            {
                return ((UITabBarController)rootController.PresentedViewController).SelectedViewController;
            }

            return rootController.PresentedViewController;
        }
    }
}
