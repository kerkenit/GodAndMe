using System;
using SQLite;

namespace GodAndMe.Models
{
    [Table("Prayers")]
    public class Prayers
    {
        [PrimaryKey, Column("id")]
        public string Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("description")]
        public string Description { get; set; }
    }
}