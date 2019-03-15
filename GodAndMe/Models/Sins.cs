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
        [Column("execution")]
        public DateTime Start { get; set; }
    }
}