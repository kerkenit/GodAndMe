using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using GodAndMe.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.Android.Interface.Settings))]
namespace GodAndMe.Android.Interface
{
    public class Settings : ISettings
    {
        private readonly Context _context;
        private readonly ISharedPreferences sharedPref;
        private readonly ISharedPreferencesEditor editor;

        public Settings()
        {
            _context = Application.Context;
            sharedPref = _context.GetSharedPreferences(CommonFunctions.APPNAME, FileCreationMode.Private);
            editor = sharedPref.Edit();
        }

        public bool GetTouchID()
        {
            return sharedPref.GetBoolean(CommonFunctions.TOUCHID, false);
        }

        public void SetTouchID(bool value)
        {
            editor.PutBoolean(CommonFunctions.TOUCHID, value);
            editor.Commit();
        }

        public string GetYourName()
        {
            return sharedPref.GetString(CommonFunctions.YOURNAME, string.Empty);
        }

        public void SetYourName(string value)
        {
            editor.PutString(CommonFunctions.YOURNAME, value);
            editor.Commit();
        }

        public int GetContactSorting()
        {
            return sharedPref.GetInt(CommonFunctions.CONTACTS_ORDERBY, 1);
        }

        public void SetContactSorting(int value)
        {
            editor.PutInt(CommonFunctions.CONTACTS_ORDERBY, value);
            editor.Commit();
        }
    }
}
