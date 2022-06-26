using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    [Table("Classification")]
    public partial class Classification
    {
        [Key]
        public int ClassificationId { get; set; }
        [StringLength(255)]
        public string ClassificationCode { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        [StringLength(255)]
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser { get; set; }
    }
}
