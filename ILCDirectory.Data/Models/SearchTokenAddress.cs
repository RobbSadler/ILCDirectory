using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class SearchTokenAddress
    {
        public int? SearchTokenAddressId { get; set; }
        public int SearchTokenId { get; set; }
        public int AddressId { get; set; }
    }
}
