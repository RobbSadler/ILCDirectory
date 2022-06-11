using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class TimeFrame
    {
        [Key]
        public int TimeframeId { get; set; }
        [StringLength(50)]
        public string TimeFrameDesc { get; set; }
    }
}
