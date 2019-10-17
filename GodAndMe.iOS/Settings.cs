using System;
using Foundation;
using LocalAuthentication;

namespace GodAndMe.iOS
{
    public enum ContactSort
    {
        Last = 1,
        First = 2
    };


    /// <summary>
    /// This class manages the system settings.
    /// </summary>
    public class Settings
    {
        public static bool TouchID { get; private set; }
        public static string YourName { get; private set; }
        public static ContactSort ContactSort { get; private set; }



        public static void LoadDefaultValues()
        {
            var settingsDict = new NSDictionary(NSBundle.MainBundle.PathForResource("Settings.bundle/Root.plist", null));

            var prefSpecifierArray = settingsDict["PreferenceSpecifiers"] as NSArray;

            foreach (var prefItem in NSArray.FromArray<NSDictionary>(prefSpecifierArray))
            {
                var key = (NSString)prefItem["Key"];
                if (key == null)
                    continue;
                if (((new LAContext()).BiometryType == LABiometryType.TouchId ? CommonFunctions.FACEID : CommonFunctions.TOUCHID) == key.ToString())
                {
                    continue;
                }
                var val = prefItem["DefaultValue"];
                switch (key.ToString())
                {
                    case CommonFunctions.FACEID:
                    case CommonFunctions.TOUCHID:
                        bool touch_id = false;
                        if (bool.TryParse(val.ToString(), out touch_id))
                        {
                            TouchID = touch_id;
                        }
                        break;
                    case CommonFunctions.YOURNAME:
                        YourName = val.ToString();
                        break;
                    case CommonFunctions.CONTACTS_ORDERBY:
                        ContactSort = (ContactSort)((NSNumber)val).Int32Value;
                        break;
                }
            }
            var appDefaults = new NSDictionary(CommonFunctions.TOUCHID, TouchID, CommonFunctions.YOURNAME, YourName, CommonFunctions.CONTACTS_ORDERBY, (int)ContactSort);

            NSUserDefaults.StandardUserDefaults.RegisterDefaults(appDefaults);
        }

        public static void SetupByPreferences()
        {
            TouchID = NSUserDefaults.StandardUserDefaults.BoolForKey(CommonFunctions.TOUCHID);
            YourName = NSUserDefaults.StandardUserDefaults.StringForKey(CommonFunctions.YOURNAME);
            ContactSort = (ContactSort)(int)NSUserDefaults.StandardUserDefaults.IntForKey(CommonFunctions.CONTACTS_ORDERBY);
        }
    }
}
