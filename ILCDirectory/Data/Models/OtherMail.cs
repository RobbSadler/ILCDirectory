using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class OtherMail
    {
        [Key]
        public int OtherMailId { get; set; }
        [StringLength(50)]
        public string Sender { get; set; }
        [StringLength(50)]
        public string Receiver { get; set; }
        public int? DeliveryCode { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }
    }
}
