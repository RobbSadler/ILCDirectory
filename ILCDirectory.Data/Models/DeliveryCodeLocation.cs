using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class DeliveryCodeLocation
    {
        public int? DeliveryCodeLocationId { get; set; }
        public string DeliveryCode { get; set; }
        public string DeliveryLocation { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
    }
}
