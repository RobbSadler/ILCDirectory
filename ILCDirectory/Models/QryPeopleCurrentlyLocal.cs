using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class QryPeopleCurrentlyLocal
    {
        [Column("ID")]
        public int Id { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(25)]
        public string NickName { get; set; }
        [StringLength(255)]
        public string OfficePhone { get; set; }
        [StringLength(255)]
        public string HomePhone { get; set; }
        [StringLength(255)]
        public string CellPhone { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ArrivalDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DepartureDate { get; set; }
        [StringLength(255)]
        public string StatusCode { get; set; }
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [StringLength(255)]
        public string CityCode { get; set; }
        [StringLength(12)]
        public string ZipCode { get; set; }
        [StringLength(255)]
        public string RoomNumber { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
        [StringLength(255)]
        public string WorkgroupCode { get; set; }
    }
}
