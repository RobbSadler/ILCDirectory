using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class QryLookUpWorkGroups
    {
        [Column("WorkgroupID")]
        public int WorkgroupId { get; set; }
        [StringLength(255)]
        public string WorkgroupCode { get; set; }
        [StringLength(255)]
        public string WorkgroupRoom { get; set; }
        [StringLength(255)]
        public string DirectoryGroupCode { get; set; }
        [StringLength(255)]
        public string WorkgroupLongDesc { get; set; }
        [StringLength(255)]
        public string WorkgroupPhone { get; set; }
        public int? ListDir { get; set; }
        [StringLength(255)]
        public string WorkgroupBuilding { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
    }
}
