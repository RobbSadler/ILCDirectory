using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class PersonFamilyDetails
    {
        public Person Spouse { get; set; }
        public IList<Person> ParentPersons { get; set; }
        public IList<Person> ChildPersons { get; set; }
        public Dictionary<int, IList<PhoneNumber>> PersonPhones { get; set; } = new Dictionary<int, IList<PhoneNumber>>();
        public Dictionary<int, IList<Email>> PersonEmails { get; set; } = new Dictionary<int, IList<Email>>();
    }
}
