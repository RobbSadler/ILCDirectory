using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class QryDirectoryCorrection
    {
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(25)]
        public string NickName { get; set; }
        [StringLength(50)]
        public string Suffix { get; set; }
        [StringLength(255)]
        public string BoxNumber { get; set; }
        [StringLength(255)]
        public string DirectoryRoom { get; set; }
        [StringLength(255)]
        public string DirectoryOfficePhone { get; set; }
        [StringLength(25)]
        public string SpouseName { get; set; }
        [StringLength(25)]
        public string SpouseNickName { get; set; }
        [StringLength(255)]
        public string Position { get; set; }
        [StringLength(255)]
        public string Supervisor { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DepartureDate { get; set; }
        [StringLength(255)]
        public string CellPhone { get; set; }
        [StringLength(255)]
        public string DirectoryAddress { get; set; }
        [StringLength(255)]
        public string DirectoryBuilding { get; set; }
        [StringLength(255)]
        public string DirectoryCity { get; set; }
        [Column("DirectoryZIP")]
        [StringLength(255)]
        public string DirectoryZip { get; set; }
        [StringLength(255)]
        public string DirectoryWorkgroup { get; set; }
        [StringLength(255)]
        public string DirectoryHomePhone { get; set; }
        [StringLength(255)]
        public string DirectoryName { get; set; }
        [StringLength(255)]
        public string HomePhone { get; set; }
        [StringLength(255)]
        public string DeliveryCode { get; set; }
        [StringLength(255)]
        public string WorkgroupLongDesc { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [StringLength(12)]
        public string ZipCode { get; set; }
        [StringLength(255)]
        public string CityCode { get; set; }
    }
}
