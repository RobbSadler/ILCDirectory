namespace ILCDirectory.Data.Models
{
    public class Address
    {
        //[Key]
        public int AddressId { get; set; }
        public bool IsActive { get; set; }
        public bool IncludeInDirectory {  get; set; }
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [StringLength(255)]
        public string AddressLine2 { get; set; }
        [StringLength(255)]
        public string AddressLine3 { get; set; }
        [StringLength(255)]
        public string AddressLine4 { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(12)]
        public string ZipCode { get; set; }
        [StringLength(255)]
        public string HomePhone { get; set; }
        [StringLength(255)]
        public string CellPhone { get; set; }
        [StringLength(255)]
        public Person ContactPerson { get; set; }
        [StringLength(255)]
        public string DepartComment { get; set; }
        [StringLength(2000)]
        public string AuditTrail { get; set; }



        public string DirectoryAddress { get; set; }
        [StringLength(255)]
        public string DirectoryBuilding { get; set; }
        [StringLength(255)]
        public string DirectoryCity { get; set; }
        [StringLength(255)]
        public string DirectoryHomePhone { get; set; }
        public bool? DirectoryInclude { get; set; }
        [StringLength(255)]
        public string DirectoryName { get; set; }
        [StringLength(255)]
        public string DirectoryOfficePhone { get; set; }
        [StringLength(255)]
        public string DirectoryRoom { get; set; }
        [StringLength(255)]
        public string DirectoryWorkgroup { get; set; }
        [StringLength(255)]
        public string DirectoryZip { get; set; }
        [StringLength(255)]
        public string BoxNumber { get; set; }

        [StringLength(255)]
        public string RoomNumber { get; set; }

        [StringLength(255)]
        public string DeliveryCode { get; set; }
        public bool? MailListFlag { get; set; }
        public bool? MailOnly { get; set; }
        [StringLength(255)]
        public string MailSortName { get; set; }
        [StringLength(255)]
        public string SpecialContactInfo { get; set; }
        [StringLength(255)]
        public string SpecialForwardingInstructions { get; set; }
        [StringLength(255)]
        public string SpecialHandlingInstructions { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
        [StringLength(255)]
        public string CubicleNumber { get; set; }
        [StringLength(255)]
        public string DirectPhone { get; set; }
        [Column("OfficeFAX")]
        [StringLength(255)]
        public string OfficeFax { get; set; }
        [StringLength(255)]
        public string OfficePhone { get; set; }

        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser {  get; set; }

    }
}
