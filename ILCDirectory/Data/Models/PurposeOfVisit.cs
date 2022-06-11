using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class PurposeOfVisit
    {
        [Key]
        public int PurposeOfVisitId { get; set; }
        [StringLength(50)]
        public string PurposeOfVisitDesc { get; set; }
    }
}
