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
        public int PersonId { get; set; }
        public PhoneNumberType PhoneNumberType { get; set; }
        public string Number { get; set; }
        public string Extension { get; set; }
        public bool IncludeInDirectory { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
