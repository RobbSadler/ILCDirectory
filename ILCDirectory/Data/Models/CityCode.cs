using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class CityCode
    {
        [Key]
        public int CityCodeId { get; set; }
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Desc { get; set; }
        [StringLength(255)]
        public string ShortDesc { get; set; }
        [StringLength(255)]
        public string AuditTrail { get; set; }
    }
}
