using Android.App;
using Android.Content;
using GodAndMe.Interface;
using System;
using System.Threading.Tasks;
using Uri = Android.Net.Uri;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.Android.Document))]
namespace GodAndMe.Android
{
    public class Document : IDocument
    {
        private readonly Context _context;
        public Document()
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

        // MUST BE CALLED FROM THE UI THREAD
        public Task<string> GetFile()
        {
            TaskCompletionSource<string> taskSource = new TaskCompletionSource<string>();

            try
            {
                taskSource.SetResult(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                taskSource.SetResult(null);
            }
            return taskSource.Task;
        }
    }
}
