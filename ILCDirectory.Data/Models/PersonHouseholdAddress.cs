using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class PersonHouseholdAddress
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MaidenName { get; set; }
        public string NickName { get; set; }
        public int HouseholdId { get; set; }
        public string HouseholdName { get; set; }
        public IList<Person> Children { get; set; }
        public Person Spouse { get; set; }
        public Address CurrentAddress { get; set; }
    }
}
