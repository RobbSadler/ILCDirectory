using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class MailDelivery
    {
        [Key]
        public int MailDeliveryId { get; set; }
        [StringLength(255)]
        public string DeliveryCode { get; set; }
        [StringLength(255)]
        public string DeliveryLocation { get; set; }
        [StringLength(2000)]
        public string AuditTrail { get; set; }
    }
}
