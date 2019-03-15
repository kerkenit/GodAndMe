using System;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Intention")]
    public class Intention
    {
        [PrimaryKey, Column("id")]
        public string Id { get; set; }
        //[Column("text")]
        //public string Text { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("person")]
        public string Person { get; set; }
        [Column("start")]
        public DateTime? Start { get; set; }
        [Column("completed_yn")]
        public bool Completed { get; set; }

        public string TextColor
        {
            get
            {
                return Completed ? "Gray" : "Black";
            }
        }

        public bool EmptyDate
        {
            get
            {
                return Start != null;
            }
        }


    }
}