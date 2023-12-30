using ILCDirectory.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class HouseholdAddress
    {
        public int? HouseholdAddressId { get; set; }
        public int HouseholdId { get; set; }
        public int? AddressId { get; set; }
        public int? InternalAddressId { get; set; }
        public bool IsPermanent { get; set; }
        public bool IncludeInDirectory { get; set; }
        public bool MailOnly { get; set; }
        public PurposeOfVisit? PurposeOfVisit { get; set; }
        public DateTimeOffset? ArrivalDate { get; set; }
        public DateTimeOffset? DepartureDate { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
