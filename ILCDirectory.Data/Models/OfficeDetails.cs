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
        /*
         * WorkGroup
         * Position
         * Supervisor
         * WoCode
         * WorkgroupCode
         * Member (IsMember?)
         * FieldOfService
         * 
         */
        [StringLength(255)]
        public string Position { get; set; }
        [StringLength(255)]
        public string Supervisor { get; set; }
        [StringLength(255)]
        public string WoCode { get; set; }
        public int? WorkgroupCode { get; set; }
        [StringLength(255)]
        //public string WorkgroupCodeOld { get; set; }
        //public bool? Member { get; set; } this is unused in source DB
        [StringLength(255)]
        public string FieldOfService { get; set; }
        public int DDDId { get; set; }

    }
}
