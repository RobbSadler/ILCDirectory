using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class Building
    {
        [Key]
        public int BuildingId { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
        [StringLength(255)]
        public string BuildingLongDesc { get; set; }
        [StringLength(255)]
        public string BuildingShortDesc { get; set; }
        [StringLength(255)]
        public string AuditTrail { get; set; }

        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser { get; set; }

    }
}
