using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class QryEmailList
    {
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(25)]
        public string FirstName { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ArrivalDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DepartureDate { get; set; }
    }
}
