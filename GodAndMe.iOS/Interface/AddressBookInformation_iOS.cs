using System.Collections.Generic;
using System.Threading.Tasks;
using AddressBook;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(GodAndMe.iOS.AddressBookInformation))]
namespace GodAndMe.iOS
{
    public class AddressBookInformation : IAddressBookInformation
    {
        ABPerson[] myContacts;

        public List<string> GetContacts()
        {
            TaskCompletionSource<List<string>> taskSource = new TaskCompletionSource<List<string>>();
            NSError error;

            ABAddressBook iPhoneAddressBook = ABAddressBook.Create(out error);
            myContacts = iPhoneAddressBook.GetPeople();

            List<string> peopleList = new List<string>();
            foreach (ABPerson contact in myContacts)
            {
                if (contact.PersonKind != ABPersonKind.Organization)
                {
                    peopleList.Add((contact.FirstName + " " + contact.MiddleName).Trim() + " " + contact.LastName);
                }
            }
            taskSource.SetResult(peopleList);
            return peopleList;
            //return taskSource.Task;
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