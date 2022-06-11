//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace ILCDirectory.Models
//{
//    [Table("Notes_Person")]
//    public partial class NotesPerson
//    {
//        [Key]
//        [Column("PersonID")]
//        public int PersonId { get; set; }
//        [StringLength(255)]
//        public string AddressLine1 { get; set; }
//        [StringLength(255)]
//        public string AddressLine2 { get; set; }
//        [StringLength(255)]
//        public string AddressLine3 { get; set; }
//        [StringLength(255)]
//        public string BoxNumber { get; set; }
//        [StringLength(255)]
//        public string Building { get; set; }
//        [StringLength(255)]
//        public string BuildingCode { get; set; }
//        [StringLength(255)]
//        public string BuildingShort { get; set; }
//        [StringLength(255)]
//        public string CellPhone { get; set; }
//        [StringLength(255)]
//        public string City { get; set; }
//        [StringLength(255)]
//        public string CityCode { get; set; }
//        [StringLength(255)]
//        public string CityShort { get; set; }
//        [StringLength(255)]
//        public string Comments { get; set; }
//        [StringLength(255)]
//        public string ContactPerson { get; set; }
//        [StringLength(255)]
//        public string ContactPhone { get; set; }
//        [StringLength(255)]
//        public string CubicleNumber { get; set; }
//        public bool? DeleteFlag { get; set; }
//        [StringLength(255)]
//        public string DeliveryCode { get; set; }
//        [StringLength(255)]
//        public string DeliveryLocation { get; set; }
//        [StringLength(255)]
//        public string DepartComment { get; set; }
//        [StringLength(255)]
//        public string DirectPhone { get; set; }
//        [StringLength(255)]
//        public string DirectoryAddress { get; set; }
//        [StringLength(255)]
//        public string DirectoryBuilding { get; set; }
//        [StringLength(255)]
//        public string DirectoryCity { get; set; }
//        [StringLength(255)]
//        public string DirectoryHomePhone { get; set; }
//        public bool? DirectoryInclude { get; set; }
//        [StringLength(255)]
//        public string DirectoryName { get; set; }
//        [StringLength(255)]
//        public string DirectoryOfficePhone { get; set; }
//        [StringLength(255)]
//        public string DirectoryRoom { get; set; }
//        [StringLength(255)]
//        public string DirectoryWorkgroup { get; set; }
//        [Column("DirectoryZIP")]
//        [StringLength(255)]
//        public string DirectoryZip { get; set; }
//        [StringLength(255)]
//        public string FieldOfService { get; set; }
//        [StringLength(255)]
//        public string FirstName { get; set; }
//        [StringLength(255)]
//        public string Gender { get; set; }
//        [StringLength(255)]
//        public string HomePhone { get; set; }
//        [StringLength(255)]
//        public string LastName { get; set; }
//        [StringLength(255)]
//        public string MaidenName { get; set; }
//        public bool? MailListFlag { get; set; }
//        public bool? MailOnly { get; set; }
//        [StringLength(255)]
//        public string MailSortName { get; set; }
//        [StringLength(255)]
//        public string MaritalStatus { get; set; }
//        [Column("MasterNameID")]
//        public int? MasterNameId { get; set; }
//        [StringLength(255)]
//        public string Member { get; set; }
//        [StringLength(255)]
//        public string MiddleInitial { get; set; }
//        [Column("NameID")]
//        public int? NameId { get; set; }
//        [StringLength(255)]
//        public string NickName { get; set; }
//        [Column("OfficeFAX")]
//        [StringLength(255)]
//        public string OfficeFax { get; set; }
//        [StringLength(255)]
//        public string OfficePhone { get; set; }
//        [StringLength(255)]
//        public string PersKwd { get; set; }
//        [StringLength(255)]
//        public string Position { get; set; }
//        [StringLength(255)]
//        public string PurposeOfVisit { get; set; }
//        [StringLength(255)]
//        public string RoomNumber { get; set; }
//        [StringLength(255)]
//        public string SpecialContactInfo { get; set; }
//        [StringLength(255)]
//        public string SpecialForwardingInstructions { get; set; }
//        [StringLength(255)]
//        public string SpecialHandlingInstructions { get; set; }
//        [StringLength(255)]
//        public string SpouseName { get; set; }
//        [StringLength(255)]
//        public string Status { get; set; }
//        [StringLength(255)]
//        public string StatusCode { get; set; }
//        [StringLength(255)]
//        public string Suffix { get; set; }
//        [StringLength(255)]
//        public string Supervisor { get; set; }
//        [StringLength(255)]
//        public string Title { get; set; }
//        [Column("WO_Code")]
//        [StringLength(255)]
//        public string WoCode { get; set; }
//        [Column("WO_Entity")]
//        [StringLength(255)]
//        public string WoEntity { get; set; }
//        [StringLength(255)]
//        public string Workgroup { get; set; }
//        [StringLength(255)]
//        public string WorkgroupCode { get; set; }
//        [StringLength(255)]
//        public string WorkgroupOrganization { get; set; }
//        [StringLength(255)]
//        public string WorkgroupShort { get; set; }
//        [StringLength(255)]
//        public string ZipCode { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? ArrivalDate { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? DepartureDate { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? Birthdate { get; set; }
//        [StringLength(2000)]
//        public string AuditTrail { get; set; }
//    }
//}
