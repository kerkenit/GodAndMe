using System;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Intention")]
    public class Intention
    {
        [PrimaryKey, Column("id")]
        public string Id { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("person")]
        public string Person { get; set; }
        [Column("start")]
        public DateTime? Start { get; set; }
        [Column("completed_yn")]
        public bool Completed { get; set; }

        private bool DarkMode
        {
            get
            {
                return false;
#if __IOS__
              //  return UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController.TraitCollection.UserInterfaceStyle == UIKit.UIUserInterfaceStyle.Dark;
#endif

            }
        }

        public string TextColor
        {
            get
            {
                return Completed ? "Gray" : (DarkMode ? "White" : "Black");
            }
        }

        public bool EmptyPerson
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Person);
            }
        }

        public bool EmptyDate
        {
            get
            {
                return Start != null;
            }
        }

        public bool EmptyDescription
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Description);
            }
        }
    }
}