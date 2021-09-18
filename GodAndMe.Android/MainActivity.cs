using System;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using Plugin.CurrentActivity;
using RuntimePermissions;

namespace GodAndMe.Droid
{


    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend, Intent.ActionView }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = CommonFunctions.DATATYPE, DataPathPattern = "*" + CommonFunctions.EXTENSION, DataHost = "*")]
    [IntentFilter(new[] { Intent.ActionView }, Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable }, DataSchemes = new[] { CommonFunctions.URLSCHEME }, DataHost = "path")]
    //[IntentFilter(new[] { "android.intent.action.VIEW" }, Categories = new[] { "android.intent.category.DEFAULT", "android.intent.category.BROWSABLE" }, DataSchemes = new[] { "https" }, DataHosts = new[] { "app.godandme" })]


    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static readonly int REQUEST_CONTACTS = 1;

        public static string[] PERMISSIONS_CONTACT = {
            Manifest.Permission.ReadContacts,
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);



            base.OnCreate(savedInstanceState);



            //string action = Intent.Action;
            //string strLink = Intent.DataString;
            //Intent intent = new Intent(Application.Context, typeof(MainActivity));
            //if (Intent.ActionView == action && !string.IsNullOrWhiteSpace(strLink))
            //{
            //    intent.SetAction(Intent.ActionView);
            //    intent.SetData(Intent.Data);
            //}


            LoadApplication(new App());

            var data = Intent?.Data?.EncodedAuthority;
            if (!String.IsNullOrEmpty(data))
            {
                foreach (string key in Intent?.Data?.QueryParameterNames)
                {
                    switch (key)
                    {
                        case "intention":
                        case "intentions":
                        case "sin":
                        case "sins":
                            string base64 = Intent?.Data?.GetQueryParameter(key);
                            Console.WriteLine(base64);
                            if ((Xamarin.Forms.Application.Current != null) && (Xamarin.Forms.Application.Current.MainPage != null))
                            // custom stuff here using different properties of the url passed in
                            {
                                // custom stuff here using different properties of the url passed in
                                ((Views.MainPage)Xamarin.Forms.Application.Current.MainPage).OpenBase64(base64);
                            }
                            break;
                        default:
#if DEBUG
                            throw new NotImplementedException(key);
#else
                            break;
#endif
                    }
                }

            }
        }

        //        public GodAndMe.Theme GetOperatingSystemTheme()
        //        {
        //            //Ensure the device is running Android Froyo or higher because UIMode was added in Android Froyo, API 8.0
        //            if (Build.VERSION.SdkInt >= BuildVersionCodes.Froyo)
        //            {
        //                var uiModeFlags = CrossCurrentActivity.Current.AppContext.Resources.Configuration.UiMode & UiMode.NightMask;

        //                switch (uiModeFlags)
        //                {
        //                    case UiMode.NightYes:
        //                        return GodAndMe.Theme.Dark;

        //                    case UiMode.NightNo:
        //                        return GodAndMe.Theme.Light;
        //                    default:
        //#if DEBUG
        //                        throw new NotSupportedException($"UiMode {uiModeFlags} not supported");
        //#else
        //                        return GodAndMe.Theme.Light;
        //#endif
        //                }
        //            }
        //            else
        //            {
        //                return GodAndMe.Theme.Light;
        //            }
        //        }


        /*
        * Callback received when a permissions request has been completed.
        */
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode == REQUEST_CONTACTS)
            {
                Console.WriteLine("Received response for contact permissions request.");

                // We have requested multiple permissions for contacts, so all of them need to be
                // checked.
                if (PermissionUtil.VerifyPermissions(grantResults))
                {
                    // All required permissions have been granted, display contacts fragment.

                }
                else
                {
                    Console.WriteLine("Contacts permissions were NOT granted.");
                }

            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
    }
}