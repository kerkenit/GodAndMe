using System;
using System.Threading.Tasks;
using Foundation;
using GodAndMe.Extensions;
using GodAndMe.Interface;
using MobileCoreServices;
using PCLStorage;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.iOS.Interface.Document))]
namespace GodAndMe.iOS.Interface
{
    public class Document : IDocument
    {
        // MUST BE CALLED FROM THE UI THREAD
        public Task<string> GetFile()
        {
            TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();
            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
            {
                UIViewController vc = GetVisibleViewController();

                var docPicker = new UIDocumentPickerViewController(new string[] { UTType.Data, UTType.Content }, UIDocumentPickerMode.Import);
                docPicker.WasCancelled += (sender, wasCancelledArgs) =>
                {
                };
                docPicker.DidPickDocumentAtUrls += (object sender, UIDocumentPickedAtUrlsEventArgs e) =>
                {
                    Console.WriteLine("url = {0}", e.Urls[0].AbsoluteString);
                    //bool success = await MoveFileToApp(didPickDocArgs.Url);
                    //var success = true;
                    string filename = e.Urls[0].LastPathComponent;
#pragma warning disable XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version
                    using (NSData dataToShare = NSFileManager.DefaultManager.Contents(e.Urls[0].Path))
                    {
                        taskSource.SetResult(dataToShare.ToString());
                    }
#pragma warning restore XI0002 // Notifies you from using newer Apple APIs when targeting an older OS version

                    //string msg = success ? string.Format("Successfully imported file '{0}'", filename) : string.Format("Failed to import file '{0}'", filename);
                    //var alertController = UIAlertController.Create("import", msg, UIAlertControllerStyle.Alert);
                    //var okButton = UIAlertAction.Create("OK", UIAlertActionStyle.Default, (obj) =>
                    //{
                    //    alertController.DismissViewController(true, null);
                    //});
                    //alertController.AddAction(okButton);
                    //vc.PresentViewController(alertController, true, null);
                };
                vc.PresentViewControllerAsync(docPicker, true);
            }
            return taskSource.Task;
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
