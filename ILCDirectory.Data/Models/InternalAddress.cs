using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class InternalAddress
    {
        public int? InternalAddressId { get; set; }
        public int PersonId { get; set; }
        [StringLength(255)]
        public string? SpecialContactInfo { get; set; }
        [StringLength(50)]
        public string? DeliveryCode { get; set; }
        public bool MailListFlag { get; set; }
        public bool MailOnly { get; set; }
        [StringLength(50)]
        public string? MailSortName { get; set; }
        [StringLength(100)]
        public string? RoomNumber { get; set; }
    }
}
