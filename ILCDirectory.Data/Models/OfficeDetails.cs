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
        [StringLength(20)]
        public int? BuildingId { get; set; }
        [StringLength(20)]
        public string RoomNumber { get; set; }
        [StringLength(50)]
        public string CubicleNumber { get; set; }

        [StringLength(255)]
        public string Position { get; set; }
        public string SupervisorPersonId { get; set; }
        [StringLength(255)]
        public string SupervisorNotes { get; set; }
        [StringLength(255)]
        public string WoCode { get; set; }
        public int? WorkgroupCode { get; set; }
        [StringLength(255)]
        public string FieldOfService { get; set; }
        public bool IncludeInDirectory { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }

    }
}
