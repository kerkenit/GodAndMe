using Android.App;
using Android.Content;
using GodAndMe.Interface;
using System.Threading.Tasks;
using Uri = Android.Net.Uri;

namespace GodAndMe.Droid.Interface
{
    public class Share : IShare
    {
        private readonly Context _context;
        public Share()
        {
            _context = Application.Context;
        }

        public Task Show(string title, string message, string url, string app)
        {

            var intent = new Intent(Intent.ActionSend);

            intent.SetType("text/plain");
            intent.PutExtra(Intent.ExtraStream, Uri.Parse(url));
            intent.PutExtra(Intent.ExtraText, app ?? string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }
    }
}
