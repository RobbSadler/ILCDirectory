using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public class Visit
    {
        /*
         * Arrival 
         * Departure
         * PurposeOfVisit
         * DepartComment
         */

        [StringLength(255)]
        public string PurposeOfVisit { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ArrivalDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DepartureDate { get; set; }

    }
}
