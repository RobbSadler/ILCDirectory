using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Data.Models
{
    public partial class Wo
    {
        [Key]
        public int WoId { get; set; }
        [StringLength(255)]
        public string WoCode { get; set; }
        [StringLength(255)]
        public string WoEntity { get; set; }
        DateTimeOffset CreateDateTime {  get; set; }
        DateTimeOffset ModifiedDateTime {  get; set; }
        string ModifiedByUserName { get; set; }
    }
}
