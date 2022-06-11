//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace ILCDirectory.Models
//{
//    [Table("Notes_Children")]
//    public partial class NotesChild
//    {
//        [Column("FamilyID")]
//        public int? FamilyId { get; set; }
//        [StringLength(255)]
//        public string Name { get; set; }
//        [StringLength(255)]
//        public string ChildName { get; set; }
//        [StringLength(255)]
//        public string Gender { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? Birthday { get; set; }
//        [StringLength(255)]
//        public string AuditTrail { get; set; }
//    }
//}
