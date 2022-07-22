using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class Vehicle
    {
        public int VehicleId { get; set; }
        public int OwnerPersonId { get; set; }
        public int? Year { get; set; }
        [StringLength(32)]
        public string Color { get; set; }
        [StringLength(32)]
        public string Make { get; set; }
        [StringLength(32)]
        public string Model { get; set; }
        [StringLength(32)]
        public string TagIssuer { get; set; }
        [StringLength(16)]
        public string TagNumber { get; set; }
        [StringLength(8)]
        public string PermitType { get; set; }
        public int? PermitNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PermitExpires { get; set; }
        [StringLength(2000)]
        public string AuditTrail { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser { get; set; }
        public int DDDId { get; set; }
    }
}
