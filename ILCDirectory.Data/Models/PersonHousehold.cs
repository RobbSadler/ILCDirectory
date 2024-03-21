using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class PersonHousehold
    {
        public int? PersonHouseholdId { get; set; }
        public int PersonId { get; set; }
        public int HouseholdId { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
