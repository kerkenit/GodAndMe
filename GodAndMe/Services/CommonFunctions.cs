﻿using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;

namespace GodAndMe
{

    public static class CommonFunctions
    {
        static CultureInfo ci = null;
        static string m_dbPath = null;
        static Lazy<ResourceManager> ResMgr = new Lazy<ResourceManager>(() => new ResourceManager("GodAndMe.Resx.AppResources", typeof(GodAndMe.Resx.AppResources).GetTypeInfo().Assembly));
        const string ResourceId = "GodAndMe.Resx.AppResources";

        public static string i18n(string Text)
        {
            return i18n(Text, null);
        }

        public static string i18n(string Text, object format = null)
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

        public static string dbPath
        {
            get
            {
                if (m_dbPath == null)
                {
                    string sqliteFilename = "GodAndMe.db3";
                    string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

#if __IOS__
                    //Get the iCloud's url
                    libraryPath = Path.Combine(libraryPath, "..", "Library");
                    libraryPath = Foundation.NSFileManager.DefaultManager.GetUrlForUbiquityContainer(null).AbsoluteString;
#endif
                    m_dbPath = Path.Combine(libraryPath, sqliteFilename);
                }
                Console.WriteLine("yyy Database path: {0}", m_dbPath);
                return m_dbPath;
            }
        }
    }
}
