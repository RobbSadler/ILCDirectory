using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class PhoneNumber
    {
        public int? PhoneNumberId { get; set; }
        public PhoneNumberType PhoneNumberTypeId { get; set; }
        public string Number { get; set; }
        public string PhoneNumberExtension { get; set; }
        public string PhoneNumberComment { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
