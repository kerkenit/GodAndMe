using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Plugin.CurrentActivity;
using RuntimePermissions;

namespace GodAndMe.Droid
{

    [Activity(Label = "@string/app_name", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
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
            LoadApplication(new App());
        }


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