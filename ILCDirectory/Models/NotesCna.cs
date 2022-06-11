//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace ILCDirectory.Models
//{
//    public partial class NotesCnas
//    {
//        [Key]
//        public int CnasId { get; set; }
//        [StringLength(255)]
//        public string Comment { get; set; }
//        [StringLength(255)]
//        public string Employee { get; set; }
//        [StringLength(255)]
//        public string FirstName { get; set; }
//        [StringLength(255)]
//        public string Gender { get; set; }
//        [StringLength(255)]
//        public string LastName { get; set; }
//        [StringLength(255)]
//        public string MaidenName { get; set; }
//        [StringLength(255)]
//        public string MaritalStatus { get; set; }
//        public int? MasterNameId { get; set; }
//        [StringLength(255)]
//        public string Member { get; set; }
//        [StringLength(255)]
//        public string MiddleInitial { get; set; }
//        [Column("NameID")]
//        public int? NameId { get; set; }
//        [StringLength(255)]
//        public string NickName { get; set; }
//        [Column("SpouseNameID")]
//        public int? SpouseNameId { get; set; }
//        [Column("WO_Code")]
//        [StringLength(255)]
//        public string WoCode { get; set; }
//        [Column("WO_Entity")]
//        [StringLength(255)]
//        public string WoEntity { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? Birthdate { get; set; }
//        [StringLength(2000)]
//        public string AuditTrail { get; set; }
//    }
//}
