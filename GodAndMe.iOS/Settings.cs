using System;
using Foundation;

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

        const string touchIDKey = "touchIDKey";
        const string yourNameKey = "yourNameKey";
        const string contactSortKey = "contactSortKey";

        public static void LoadDefaultValues()
        {
            var settingsDict = new NSDictionary(NSBundle.MainBundle.PathForResource("Settings.bundle/Root.plist", null));

            var prefSpecifierArray = settingsDict["PreferenceSpecifiers"] as NSArray;

            foreach (var prefItem in NSArray.FromArray<NSDictionary>(prefSpecifierArray))
            {
                var key = (NSString)prefItem["Key"];
                if (key == null)
                    continue;

                var val = prefItem["DefaultValue"];
                switch (key.ToString())
                {
                    case touchIDKey:
                        bool touch_id = false;
                        if (bool.TryParse(val.ToString(), out touch_id))
                        {
                            TouchID = touch_id;
                        }
                        break;
                    case yourNameKey:
                        YourName = val.ToString();
                        break;
                    case contactSortKey:
                        ContactSort = (ContactSort)((NSNumber)val).Int32Value;
                        break;
                }
            }
            var appDefaults = new NSDictionary(touchIDKey, TouchID, yourNameKey, YourName, contactSortKey, (int)ContactSort);

            NSUserDefaults.StandardUserDefaults.RegisterDefaults(appDefaults);
        }

        public static void SetupByPreferences()
        {
            TouchID = NSUserDefaults.StandardUserDefaults.BoolForKey(touchIDKey);
            YourName = NSUserDefaults.StandardUserDefaults.StringForKey(yourNameKey);
            ContactSort = (ContactSort)(int)NSUserDefaults.StandardUserDefaults.IntForKey(contactSortKey);
        }
    }
}
