using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GodAndMe.iOS.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }
        public static bool TouchID
        {
            get
            {
                return GodAndMe.iOS.Settings.TouchID;
            }
        }

        public static int ContactSort
        {
            get
            {
                return (int)GodAndMe.iOS.Settings.ContactSort;
            }
        }

        public static string YourName
        {
            get
            {
                return GodAndMe.iOS.Settings.YourName;
            }
        }
    }
}
