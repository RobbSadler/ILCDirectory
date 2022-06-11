using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class DtProperty
    {
        [Key]
        public int DtPropertyId { get; set; }
        public int? ObjectId { get; set; }
        [Key]
        [StringLength(64)]
        public string Property { get; set; }
        [StringLength(255)]
        public string Value { get; set; }
        [StringLength(255)]
        public string UValue { get; set; }
        [Column(TypeName = "image")]
        public byte[] LValue { get; set; }
        public int Version { get; set; }
    }
}
