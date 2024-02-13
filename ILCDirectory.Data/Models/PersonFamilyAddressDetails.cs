using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class PersonFamilyAddressDetails
    {
        // Person Family Details
        public PersonFamilyDetails PersonFamilyDetails { get; set; }

        // Address Details
        public PersonAddressDetails PersonAddressDetails { get; set; }
    }
}
