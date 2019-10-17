using System;
using System.Threading.Tasks;
using Foundation;
using GodAndMe.DependencyServices;
using LocalAuthentication;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.iOS.TouchID))]
namespace GodAndMe.iOS
{
    public class TouchID : ITouchID
    {
        static NSObject Invoker = new NSObject();

        public Task<bool> AuthenticateUserIDWithTouchID()
        {
            var taskSource = new TaskCompletionSource<bool>();
            if (Settings.TouchID && !App.justUnlocked)
            {
                LAContext context = new LAContext();
                if (context.CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out NSError AuthError))
                {
                    try
                    {
                        context.EvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics,
                            CommonFunctions.i18n("AuthenticationWithBiometricsMessage"),
                            new LAContextReplyHandler((success, error) =>
                        {
                            if (error == null)
                            {
                                App.justUnlocked = success;
                                taskSource.SetResult(success);
                            }
                            else
                            {
                                taskSource.SetResult(false);
                            }
                        }));
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return taskSource.Task;
        }


        public bool CanAuthenticateUserIDWithTouchID()
        {
            try
            {
                return Settings.TouchID && (new LAContext()).CanEvaluatePolicy(LAPolicy.DeviceOwnerAuthenticationWithBiometrics, out NSError AuthError);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(ex.StackTrace);
                return false;
            }
        }

        public static void InvokedOnMainThread(Action Action)
        {
            if (NSThread.Current.IsMainThread)
            {
                Action();
            }
            else
            {
                Invoker.BeginInvokeOnMainThread(Action);
            }
        }


    }
}