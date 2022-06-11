using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class QryLookUpPerson
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(25)]
        public string FirstName { get; set; }
        [StringLength(25)]
        public string NickName { get; set; }
        [StringLength(25)]
        public string MaidenName { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string OfficePhone { get; set; }
        [StringLength(255)]
        public string HomePhone { get; set; }
        [StringLength(12)]
        public string ZipCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ArrivalDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DepartureDate { get; set; }
        public int? WorkgroupCode { get; set; }
        [StringLength(25)]
        public string SpouseFirst { get; set; }
        [StringLength(25)]
        public string SpouseName { get; set; }
        [StringLength(255)]
        public string FieldOfService { get; set; }
        [StringLength(255)]
        public string LanguagesSpoken { get; set; }
        [StringLength(255)]
        public string StatusCode { get; set; }
        [StringLength(255)]
        public string Workgroup { get; set; }
    }
}
