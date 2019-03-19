using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace GodAndMe
{
    public static class CommonFunctions
    {
        static CultureInfo ci = null;
        static Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager("GodAndMe.Resx.AppResources", typeof(GodAndMe.Resx.AppResources).GetTypeInfo().Assembly));
        const string ResourceId = "GodAndMe.Resx.AppResources";
        public const string URLSHEME = "GodAndMe://";
        public const string APPNAME = "GodAndMe";
        public const string TOUCHID = "touchIDKey";
        public const string YOURNAME = "yourNameKey";
        public const string CONTACTS_ORDERBY = "contactSortKey";
#if DEBUG
#if __ANDROID__
        public const bool SCREENSHOT = false;
#elif __iOS__
        public const bool SCREENSHOT = false;
#endif
#else
        public const bool SCREENSHOT = false;
#endif

        public static string i18n(string Text)
        {
            return i18n(Text, null);
        }

        public static string i18n(string Text, object format)
        {
            if (ci == null && Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            }

            if (Text == null)
                return string.Empty;
            var translation = ResMgr.Value.GetString(Text, ci);
            if (translation == null)
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
#else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return translation;
        }
    }
}