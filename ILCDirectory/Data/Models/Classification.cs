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
        public string StatusCode { get; set; }
        [StringLength(255)]
        public string StatusDescription { get; set; }
        [StringLength(255)]
        public string StatusExplanation { get; set; }
        [StringLength(255)]
        public string AuditTrail { get; set; }
    }
}
