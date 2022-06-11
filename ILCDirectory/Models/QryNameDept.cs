using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class QryNameDept
    {
        [StringLength(25)]
        public string FirstName { get; set; }
        [StringLength(25)]
        public string NickName { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(255)]
        public string DirectoryWorkgroup { get; set; }
        [StringLength(255)]
        public string DirectoryBuilding { get; set; }
        [StringLength(255)]
        public string DirectoryRoom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Arrival { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DepartureDate { get; set; }
    }
}
