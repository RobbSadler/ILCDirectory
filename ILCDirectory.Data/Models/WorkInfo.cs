using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public class WorkInfo
    {
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
        public string WorkgroupCodeOld { get; set; }
        public bool? Member { get; set; }
        [StringLength(255)]
        public string FieldOfService { get; set; }
        public int DDDId {  get; set; }
    }
}
