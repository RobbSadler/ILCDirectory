using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    [Table("tblTitle")]
    public partial class Title
    {
        [Key]
        [Column("TitleID")]
        public int TitleId { get; set; }
        [StringLength(10)]
        public string TitleName { get; set; }
    }
}
