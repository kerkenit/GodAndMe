using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GodAndMe.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;


        public static bool TouchID
        {
            get => AppSettings.GetValueOrDefault(nameof(TouchID), false);
        }

        public static int ContactSort
        {
            get => AppSettings.GetValueOrDefault(nameof(ContactSort), 1);
        }

        public static string YourName
        {
            get => AppSettings.GetValueOrDefault(nameof(YourName), string.Empty);
        }
    }
}
