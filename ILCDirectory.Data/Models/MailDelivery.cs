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
        public string BoxNumber { get; set; }
        public bool IncludeInSort { get; set; }
        public bool InMailListsOnly { get; set; }
        [StringLength(255)]
        public string SpecialContactInfo { get; set; }
        [StringLength(255)]
        public string SpecialForwardingInstructions { get; set; }
        [StringLength(255)]
        public string SpecialHandlingInstructions { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }
    }
}
