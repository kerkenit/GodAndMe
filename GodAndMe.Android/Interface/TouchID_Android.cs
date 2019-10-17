using System.Threading.Tasks;
using Android.App;
using Android.Content;
using GodAndMe.DependencyServices;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.Android.TouchID))]
namespace GodAndMe.Android
{
    public class TouchID : ITouchID
    {
        private readonly Context _context;

        public TouchID()
        {
            _context = Application.Context;
        }

        public Task<bool> AuthenticateUserIDWithTouchID()
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();
            taskSource.SetResult(false);
            return taskSource.Task;
        }

        public bool CanAuthenticateUserIDWithTouchID()
        {
            return false;
        }



        public LocalAuthType GetLocalAuthType()
        {
            return LocalAuthType.None;
        }
    }
}