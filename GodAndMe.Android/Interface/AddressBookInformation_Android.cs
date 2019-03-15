﻿using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Android.Provider;
using Android.App;
using System;
using Plugin.CurrentActivity;
using Android;
using RuntimePermissions;
using Android.Util;
using Android.Support.Design.Widget;
using Android.Views;
using Android.Content.PM;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.Android.AddressBookInformation))]
namespace GodAndMe.Android
{
    public class AddressBookInformation : IAddressBookInformation
    {
        public static readonly int REQUEST_CONTACTS = 1;

        public static string[] PERMISSIONS_CONTACT = {
            Manifest.Permission.ReadContacts
        };

        /// <summary>
        /// Gets or sets the activity.
        /// </summary>
        /// <value>The activity.</value>
        Activity Activity
        {
            get
            {
                return CrossCurrentActivity.Current.Activity;
            }
        }

        Task<List<string>> IAddressBookInformation.GetContacts()
        {
            TaskCompletionSource<List<string>> taskSource = new TaskCompletionSource<List<string>>();

            List<string> items = new List<string>();

            Dictionary<string, string> peopleList = new Dictionary<string, string>();


            global::Android.Net.Uri uriContacts = ContactsContract.Contacts.ContentUri;

            string[] projection = {
                ContactsContract.Contacts.InterfaceConsts.Id,
                ContactsContract.Contacts.InterfaceConsts.DisplayName
            };


            if (Activity == null)
            {
                throw new Exception("You have to set ScreenshotManager.Activity in your Android project");
            }

            // Verify that all required contact permissions have been granted.
            if (CrossCurrentActivity.Current.Activity.CheckSelfPermission(Manifest.Permission.ReadContacts) != (int)Permission.Granted)

            {
                // Contacts permissions have not been granted.
                Console.WriteLine("Contact permissions has NOT been granted. Requesting permissions.");
                RequestAccess();
            }
            else
            {

                // Contact permissions have been granted. Show the contacts fragment.
                Console.WriteLine("Contact permissions have already been granted. Displaying contact details.");
                global::Android.Database.ICursor cursor = Activity.ManagedQuery(uriContacts, projection, null, null, null);

                if (cursor.MoveToFirst())
                {
                    do
                    {
                        string key = cursor.GetString(cursor.GetColumnIndex(projection[1])).Trim();

                        if (!peopleList.ContainsKey(key))
                        {
                            peopleList.Add(key, key);
                        }


                    } while (cursor.MoveToNext());
                }

                List<string> list = peopleList.Keys.ToList();
                list.Sort();

                foreach (string key in list)
                {
                    if (!string.IsNullOrWhiteSpace(peopleList[key]))
                    {
                        items.Add(peopleList[key]);
                    }
                }

                taskSource.SetResult(items);
            }
            //return items;
            return taskSource.Task;
        }
        /*
        * Requests the Contacts permissions.
        * If the permission has been denied previously, a SnackBar will prompt the user to grant the
        * permission, otherwise it is requested directly.
        */
        public Task<bool> RequestAccess()
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();

            if (CrossCurrentActivity.Current.Activity.ShouldShowRequestPermissionRationale(Manifest.Permission.ReadContacts))
            {

                // Provide an additional rationale to the user if the permission was not granted
                // and the user would benefit from additional context for the use of the permission.
                // For example, if the request has been denied previously.
                Console.WriteLine("Displaying contacts permission rationale to provide additional context.");

                //ProfileView pv = new ProfileView(this, null, temp, tempPd);
                //// Display a SnackBar with an explanation and a button to trigger the request.
                //Snackbar.Make(CrossCurrentActivity.Current.Activity.FindViewById(GetWindow().getDecorView().getRootView()).getRootView(), Resource.String.permission_contacts_rationale,
                //Snackbar.LengthIndefinite).SetAction(Resource.String.ok, new Action<View>(delegate (View obj)
                //{
                //    CrossCurrentActivity.Current.Activity.RequestPermissions(PERMISSIONS_CONTACT, REQUEST_CONTACTS);
                //})).Show();
            }
            else
            {
                // Contact permissions have not been granted yet. Request them directly.
                CrossCurrentActivity.Current.Activity.RequestPermissions(PERMISSIONS_CONTACT, REQUEST_CONTACTS);
            }
            taskSource.SetResult(true);

            return taskSource.Task;
        }


    }
}