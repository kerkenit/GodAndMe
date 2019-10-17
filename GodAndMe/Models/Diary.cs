using System;
using SQLite;

namespace GodAndMe.Models
{
    public enum DiaryType
    {
        God,
        DiscernmentOfSpirits,
        Personal
    }
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
        [Column("type")]
        public int DiaryType { get; set; }

        public string DiaryTypeText
        {
            get
            {
                return CommonFunctions.i18n(((DiaryType)Enum.Parse(typeof(DiaryType), DiaryType.ToString())).ToString());
            }
        }

    }
}