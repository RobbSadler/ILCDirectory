using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Models
{
    public class OfficeDetails
    {
        public int OfficeDetailsId {  get; set; }
        public int PersonId { get; set; }
        public int? BuildingId { get; set; }
        [StringLength(20)]
        public string RoomNumber { get; set; }
        [StringLength(50)]
        public string CubicleNumber { get; set; }
        public bool IncludeInDirectory { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }

    }
}
