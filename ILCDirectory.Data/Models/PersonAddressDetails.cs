using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class PersonAddressDetails
    {
        public IList<PersonHousehold> Households { get; set; }
        public IList<HouseholdAddress> HouseholdAddresses { get; set; }
        public IList<Address> PersonAddresses { get; set; }
    }
}
