using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBook;
using Foundation;
using UIKit;
using System.Linq;
using Contacts;
using ContactsUI;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.iOS.AddressBookInformation))]
namespace GodAndMe.iOS
{
    public class AddressBookInformation : IAddressBookInformation
    {
        ABPerson[] myContacts;

        Task<List<string>> IAddressBookInformation.GetContacts()
        {
            TaskCompletionSource<List<string>> taskSource = new TaskCompletionSource<List<string>>();
            NSError error;
            List<string> items = new List<string>();

            if (false && UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                UIViewController yourController = UIApplication.SharedApplication.KeyWindow.RootViewController;


                // Create a new picker
                var picker = new CNContactPickerViewController();
                // Select property to pick
                picker.DisplayedPropertyKeys = new NSString[] { CNContactKey.GivenName };
                //picker.PredicateForEnablingContact = NSPredicate.FromFormat("emailAddresses.@count > 0");
                // picker.PredicateForSelectionOfContact = NSPredicate.FromFormat("emailAddresses.@count == 1");
                var contactPickerDelegate = new ContactPickerDelegate();

                // Respond to selection
                picker.Delegate = contactPickerDelegate;
                // Display picker
                yourController.PresentViewController(picker, true, null);
            }
            else
            {
                ABAddressBook iPhoneAddressBook = ABAddressBook.Create(out error);
                myContacts = iPhoneAddressBook.GetPeople();

                Dictionary<string, string> peopleList = new Dictionary<string, string>();
                foreach (ABPerson contact in myContacts)
                {
                    if (contact.PersonKind == ABPersonKind.Person && contact.Type == ABRecordType.Person)
                    {
                        string key = ((contact.LastName + " " + contact.MiddleName).Trim() + " " + contact.FirstName).Trim();
                        if (!peopleList.ContainsKey(key))
                        {
                            peopleList.Add(key, (contact.FirstName + " " + contact.MiddleName).Trim() + " " + contact.LastName);
                        }
                    }
                }
                List<string> list = peopleList.Keys.ToList();
                list.Sort();

                foreach (string key in list)
                {
                    if (!string.IsNullOrEmpty(peopleList[key]))
                    {
                        items.Add(peopleList[key]);
                    }
                }
            }
            taskSource.SetResult(items);
            //return items;
            return taskSource.Task;
        }


        public Task<bool> RequestAccess()
        {
            TaskCompletionSource<bool> taskSource = new TaskCompletionSource<bool>();

            NSError err;
            ABAddressBook iPhoneAddressBook = ABAddressBook.Create(out err);
            var authStatus = ABAddressBook.GetAuthorizationStatus();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                if (authStatus != ABAuthorizationStatus.Authorized)
                {
                    iPhoneAddressBook.RequestAccess(delegate (bool granted, NSError error)
                    {
                        taskSource.SetResult(granted);
                    });
                }
                else if (authStatus == ABAuthorizationStatus.Authorized)
                {
                    taskSource.SetResult(true);
                }
            }
            else
            {
                taskSource.SetResult(true);
            }
            return taskSource.Task;
        }
    }
}