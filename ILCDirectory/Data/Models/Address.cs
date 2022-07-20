namespace ILCDirectory.Data.Models
{
    public class Address
    {
        public int AddressId { get; set; }
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
        public int ContactPersonId { get; set; }
        [StringLength(255)]
        public string SpecialContactInfo { get; set; }
        [StringLength(255)]
        public string SpecialForwardingInstructions { get; set; }
        [StringLength(255)]
        public string SpecialHandlingInstructions { get; set; }
        [StringLength(2000)]
        public string AuditTrail { get; set; }
        [StringLength(255)]
        public string BoxNumber { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
        [StringLength(255)]
        public string CubicleNumber { get; set; }
        [StringLength(255)]
        public string DeliveryCode { get; set; }
        [StringLength(255)]
        public string MailSortName { get; set; }
        [StringLength(255)]
        public string RoomNumber { get; set; }
        public bool? MailListFlag { get; set; }
        public bool? MailOnly { get; set; }
        public bool IncludeInDirectory {  get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName {  get; set; }

    }
}
