using System;
using System.Threading.Tasks;
using Foundation;
using GodAndMe.Interface;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.iOS.Interface.Settings))]
namespace GodAndMe.iOS.Interface
{
    public class Settings : ISettings
    {
        public bool GetTouchID()
        {
            return NSUserDefaults.StandardUserDefaults.BoolForKey(CommonFunctions.TOUCHID);
        }

        public void SetTouchID(bool value)
        {
            NSUserDefaults.StandardUserDefaults.SetBool(value, CommonFunctions.TOUCHID);
        }

        public string GetYourName()
        {
            return NSUserDefaults.StandardUserDefaults.StringForKey(CommonFunctions.YOURNAME);
        }

        public void SetYourName(string value)
        {
            NSUserDefaults.StandardUserDefaults.SetString(value, CommonFunctions.YOURNAME);
        }

        public int GetContactSorting()
        {
            return (int)NSUserDefaults.StandardUserDefaults.IntForKey(CommonFunctions.CONTACTS_ORDERBY);
        }

        public void SetContactSorting(int value)
        {
            NSUserDefaults.StandardUserDefaults.SetInt(value, CommonFunctions.CONTACTS_ORDERBY);
        }
    }
}
