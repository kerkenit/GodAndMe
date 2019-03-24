using Android.App;
using Android.Content;
using GodAndMe.Interface;
using System;
using System.Threading.Tasks;
using Uri = Android.Net.Uri;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.Android.Share))]
namespace GodAndMe.Android
{
    public class Share : IShare
    {
        private readonly Context _context;
        public Share()
        {
            try
            {
                _context = Application.Context;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public Task Show(string title, string message, string url, string app)
        {
            try
            {
                Intent intent = new Intent(Intent.ActionSend);

                intent.SetType("text/plain");
                intent.PutExtra(Intent.ExtraText, title + Environment.NewLine + message + Environment.NewLine + Environment.NewLine + url + app);
                intent.PutExtra(Intent.ExtraSubject, title ?? string.Empty);
                Intent chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
                chooserIntent.SetFlags(ActivityFlags.ClearTop);
                chooserIntent.SetFlags(ActivityFlags.NewTask);
                _context.StartActivity(chooserIntent);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Task.FromResult(false);
            }
        }
    }
}
