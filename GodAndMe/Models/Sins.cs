using System;
using System.ComponentModel;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Sins")]
    public class Sins : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

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