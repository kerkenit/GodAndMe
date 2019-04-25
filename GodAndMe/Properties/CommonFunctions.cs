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
        public static CultureInfo Culture
        {
            get
            {
#if __IOS__
                if (ci == null)
                {
                    ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                }
#elif __ANDROID__
                if (ci == null)
                {
                    ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                }
#endif
                if (ci == null)
                {
                    ci = new CultureInfo("en-US");
                }
                return ci;
            }
        }
        static Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager("GodAndMe.Resx.AppResources", typeof(GodAndMe.Resx.AppResources).GetTypeInfo().Assembly));
        const string ResourceId = "GodAndMe.Resx.AppResources";
        public const string URLSHEME = "godandme://";
        public const string URLSCHEME = "godandme";
        public const string EXTENSION = ".god";
        public const string DATATYPE = "application/godandme";
        public const string APPNAME = "GodAndMe";
        public const string TOUCHID = "touchIDKey";
        public const string YOURNAME = "yourNameKey";
        public const string CONTACTS_ORDERBY = "contactSortKey";
        public static string[] CRYPTOKEY = { "MIIBYgIBADANBgkqhkiG9w0BAQEFAASCAT0wggE5AgEAAkEAxw78Hsu1rGDmVSeMvclLNGTN+jOeTXdmV+VOe/od1cnoFko8aKolSrfsVdPqsUWZAWvHr7qgzuGn1NGhDt4G+wIDAQABAkAal+5n4Ng7GND81GVRn5hb/hGkmQvPlqGGIZzsJDyjKHMK5Ky9JCaddhkKwfojfJynC9ghKqjP8u9h0Oby9JINAiEA/iXzYnZyQ686WRtPvhtAz6eVHcQShjAMv81KN07J6r0CIQDIgkc4KKoB3nwkbri4IQqsj4zcJwR8BrWy7KozEDiwFwIgL0GrQdG4aXF5rfvwFe9HW9VTWteMgjsJA9kORb52uRkCIFWdy2tfcbh6l+e2n4mAEl68rRkUUAXll5BfHg3Pz2ThAiBIPBY4ihP0X5zKRmBdr9+eknCQb+s57Vmizy3+niLbpqANMAsGA1UdDzEEAwIAEA==", "MEgCQQDHDvwey7WsYOZVJ4y9yUs0ZM36M55Nd2ZX5U57+h3VyegWSjxoqiVKt+xV0+qxRZkBa8evuqDO4afU0aEO3gb7AgMBAAE=" };
#if DEBUG
#if __ANDROID__
        public const bool SCREENSHOT = true;
#elif __SIMULATOR__
        public const bool SCREENSHOT = false;
#elif __IOS__
            public const bool SCREENSHOT = false;
#endif
#else
        public const bool SCREENSHOT = false;
#endif

        public const int LentenSavingOffset = 7;

        public static class LentenPeriod
        {
            public static DateTime Start
            {
                get
                {
                    return EasterSunday(DateTime.Today.Year).AddDays(-46).AddDays(LentenSavingOffset * -1);
                }
            }

            public static DateTime End
            {
                get
                {
                    return EasterSunday(DateTime.Today.Year).AddDays(LentenSavingOffset);
                }
            }
        }

        public static DateTime EasterSunday(int year)
        {
            int day = 0;
            int month = 0;

            int g = year % 19;
            int c = year / 100;
            int h = (c - (int)(c / 4) - (int)((8 * c + 13) / 25) + 19 * g + 15) % 30;
            int i = h - (int)(h / 28) * (1 - (int)(h / 28) * (int)(29 / (h + 1)) * (int)((21 - g) / 11));

            day = i - ((year + (int)(year / 4) + i + 2 - c + (int)(c / 4)) % 7) + 28;
            month = 3;

            if (day > 31)
            {
                month++;
                day -= 31;
            }

            return new DateTime(year, month, day);
        }

        public static string i18n(string Text)
        {
            return i18n(Text, null);
        }

        public static string i18n(string Text, object format)
        {


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