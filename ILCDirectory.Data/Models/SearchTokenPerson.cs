using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class SearchTokenPerson
    {
        public int? SearchTokenPersonId { get; set; }
        public int SearchTokenId { get; set; }
        public int PersonId { get; set; }
    }
}
