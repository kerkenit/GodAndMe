using System;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Diary")]
    public class Diary
    {
        [PrimaryKey, Column("id")]
        public string Id { get; set; }
        [Column("positive")]
        public string Positive { get; set; }
        [Column("negative")]
        public string Negative { get; set; }
        [Column("feeling")]
        public string Feeling { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("execution")]
        public DateTime Start { get; set; }
    }
}