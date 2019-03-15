using System;
using System.Threading.Tasks;
using GodAndMe.DependencyServices;


[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.Android.TouchID))]
namespace GodAndMe.Android
{
    public class TouchID : ITouchID
    {
        public Task<bool> AuthenticateUserIDWithTouchID()
        {
            var taskSource = new TaskCompletionSource<bool>();
            taskSource.SetResult(false);
            return taskSource.Task;
        }

        public bool CanAuthenticateUserIDWithTouchID()
        {
            return false;
        }
    }
}