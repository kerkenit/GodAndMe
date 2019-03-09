using System;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Lent")]
    public class Lent
    {
        [PrimaryKey, Column("id")]
        public string Id { get; set; }
        [Column("text")]
        public string Text { get; set; }
        [Column("from")]
        public double? MoneyFrom { get; set; }
        [Column("to")]
        public double? MoneyTo { get; set; }
        [Column("start")]
        public DateTime Start { get; set; }

        public double SavedMoney
        {
            get
            {
                return (MoneyFrom == null ? 0 : (double)MoneyFrom) - (MoneyTo == null ? 0 : (double)MoneyTo);
            }
        }

        public string TextColor
        {
            get
            {
                return SavedMoney == 0 ? "Gray" : "Black";
            }
        }
    }
}