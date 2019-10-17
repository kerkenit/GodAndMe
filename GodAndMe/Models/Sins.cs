using System;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Sins")]
    public class Sins
    {
        [PrimaryKey, Column("id")]
        public string Id { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("committed")]
        public DateTime Committed { get; set; }
        [Column("confessed_yn")]
        public bool Confessed { get; set; }
        [Column("count")]
        public int Count { get; set; }
        [Column("lastcommitted")]
        public DateTime LastCommitted { get; set; }

        private bool DarkMode
        {
            get
            {
                return false;
#if __IOS__
                return UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController.TraitCollection.UserInterfaceStyle == UIKit.UIUserInterfaceStyle.Dark;
#endif

            }
        }

        public string TextColor
        {
            get
            {
                return Confessed ? "Gray" : (DarkMode ? "White" : "Black");
            }
        }
    }
}