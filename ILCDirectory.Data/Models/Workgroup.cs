using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class Workgroup
    {
        [Key]
        public int? WorkgroupId { get; set; }
        [StringLength(255)]
        public string WorkgroupCode { get; set; }
        [StringLength(255)]
        public string Building { get; set; }
        [StringLength(255)]
        public string Room { get; set; }
        [StringLength(255)]
        public string LongDesc { get; set; }
        [StringLength(255)]
        public string DirectoryGroupCode { get; set; }
        [StringLength(255)]
        public string Organization { get; set; }
        [StringLength(255)]
        public string Phone { get; set; }
        public bool? DirectoryTab { get; set; }
        public int? ListDir { get; set; }
        [StringLength(2000)]
        public string Notes { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }
    }
}
