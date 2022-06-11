//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace ILCDirectory.Models
//{
//    [Table("Notes_Registrations")]
//    public partial class NotesRegistration
//    {
//        [StringLength(255)]
//        public string RegLastName { get; set; }
//        [StringLength(255)]
//        public string RegFirstName { get; set; }
//        [StringLength(255)]
//        public string RegMiddleInitial { get; set; }
//        [StringLength(255)]
//        public string RegNickName { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? RegBirthday { get; set; }
//        [StringLength(255)]
//        public string RegGender { get; set; }
//        [StringLength(255)]
//        public string RegMaritalStatus { get; set; }
//        [StringLength(255)]
//        public string RegMaidenName { get; set; }
//        [Column("RegMasterNameID")]
//        public int? RegMasterNameId { get; set; }
//        [Column("RegNameID")]
//        public int? RegNameId { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? RegArrivalDate { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? RegDepartureDate { get; set; }
//        [StringLength(255)]
//        public string RegBuilding { get; set; }
//        [StringLength(255)]
//        public string RegRoomNumber { get; set; }
//        [StringLength(255)]
//        public string RegOfficePhone { get; set; }
//        [StringLength(255)]
//        public string RegWorkgroup { get; set; }
//        [StringLength(255)]
//        public string RegDeliveryCode { get; set; }
//        [StringLength(255)]
//        public string RegBoxNumber { get; set; }
//        [StringLength(255)]
//        public string RegWorkGroupOrganization { get; set; }
//        [StringLength(255)]
//        public string RegStatus { get; set; }
//        [StringLength(255)]
//        public string RegPurposeofVisit { get; set; }
//        [StringLength(255)]
//        public string RegFieldOfService { get; set; }
//        [StringLength(255)]
//        public string RegComments { get; set; }
//        [StringLength(255)]
//        public string RegSpouseLastName { get; set; }
//        [StringLength(255)]
//        public string RegSpouseFirstName { get; set; }
//        [StringLength(255)]
//        public string RegSpouseMiddleInitial { get; set; }
//        [StringLength(255)]
//        public string RegSpouseNickName { get; set; }
//        [StringLength(255)]
//        public string RegSpouseBirthday { get; set; }
//        [StringLength(255)]
//        public string RegSpouseGender { get; set; }
//        [StringLength(255)]
//        public string RegSpouseMaritalStatus { get; set; }
//        [StringLength(255)]
//        public string RegSpouseMaidenName { get; set; }
//        [Column("RegSpouseMasterNameID")]
//        [StringLength(255)]
//        public string RegSpouseMasterNameId { get; set; }
//        [Column("RegSpouseNameID")]
//        [StringLength(255)]
//        public string RegSpouseNameId { get; set; }
//        [StringLength(255)]
//        public string RegSpouseArrivalDate { get; set; }
//        [StringLength(255)]
//        public string RegSpouseDepartureDate { get; set; }
//        [StringLength(255)]
//        public string RegSpouseBuilding { get; set; }
//        [StringLength(255)]
//        public string RegSpouseRoomNumber { get; set; }
//        [StringLength(255)]
//        public string RegSpouseOfficePhone { get; set; }
//        [StringLength(255)]
//        public string RegSpouseWorkGroup { get; set; }
//        [StringLength(255)]
//        public string RegSpouseDeliveryCode { get; set; }
//        [StringLength(255)]
//        public string RegSpouseBoxNumber { get; set; }
//        [StringLength(255)]
//        public string RegSpouseWorkGroupOrganization { get; set; }
//        [StringLength(255)]
//        public string RegSpouseStatus { get; set; }
//        [StringLength(255)]
//        public string RegSpousePurposeofVisit { get; set; }
//        [StringLength(255)]
//        public string RegSpouseFieldOfService { get; set; }
//        [StringLength(255)]
//        public string RegSpouseComments { get; set; }
//        [StringLength(255)]
//        public string RegContactPerson { get; set; }
//        [StringLength(255)]
//        public string RegContactPhone { get; set; }
//        [StringLength(255)]
//        public string RegHomePhone { get; set; }
//        [StringLength(255)]
//        public string RegAddrLine1 { get; set; }
//        [StringLength(255)]
//        public string RegAddrLine2 { get; set; }
//        [StringLength(255)]
//        public string RegAddrLine3 { get; set; }
//        [StringLength(255)]
//        public string RegCity { get; set; }
//        public int? RegZipCode { get; set; }
//        [StringLength(255)]
//        public string RegChild { get; set; }
//        [StringLength(255)]
//        public string RegChildBirthday { get; set; }
//        [StringLength(255)]
//        public string RegChildGender { get; set; }
//        [StringLength(255)]
//        public string RegComplete { get; set; }
//        [Column("RegDocID")]
//        [StringLength(255)]
//        public string RegDocId { get; set; }
//        [StringLength(255)]
//        public string RegProcessedBy { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? RegProcessedDate { get; set; }
//        [StringLength(255)]
//        public string RegBuildingCode { get; set; }
//        [StringLength(255)]
//        public string RegWorkgroupCode { get; set; }
//        [StringLength(255)]
//        public string RegDeliveryLocation { get; set; }
//        [StringLength(255)]
//        public string RegStatusCode { get; set; }
//        [StringLength(255)]
//        public string RegSpouseBuildingCode { get; set; }
//        [StringLength(255)]
//        public string RegSpouseWorkgroupCode { get; set; }
//        [StringLength(255)]
//        public string RegSpouseDeliveryLocation { get; set; }
//        [StringLength(255)]
//        public string RegSpouseStatusCode { get; set; }
//        [StringLength(255)]
//        public string RegCityCode { get; set; }
//        [StringLength(255)]
//        public string RegBuildingShort { get; set; }
//        [StringLength(255)]
//        public string RegSpouseBuildingShort { get; set; }
//        [Column(TypeName = "datetime")]
//        public DateTime? RegDate { get; set; }
//        [StringLength(255)]
//        public string RegTime { get; set; }
//        [StringLength(255)]
//        public string RegEnteredBy { get; set; }
//    }
//}
