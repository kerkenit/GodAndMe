using System;
using System.Linq;
using UIKit;
using Foundation;
using Contacts;
using ContactsUI;

namespace GodAndMe
{
    public class ContactPickerDelegate : CNContactPickerDelegate
    {
        #region Constructors
        public ContactPickerDelegate()
        {
        }

        public ContactPickerDelegate(IntPtr handle) : base(handle)
        {
        }
        #endregion

        #region Override Methods
        public override void ContactPickerDidCancel(CNContactPickerViewController picker)
        {
            Console.WriteLine("User canceled picker");

        }
        public override void DidSelectContact(CNContactPickerViewController picker, CNContact contact)
        {
            Console.WriteLine("Selected: {0}", contact);
        }

        public override void DidSelectContactProperty(CNContactPickerViewController picker, CNContactProperty contactProperty)
        {
            Console.WriteLine("Selected Property: {0}", contactProperty);
        }
        #endregion
    }
}