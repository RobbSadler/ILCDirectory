namespace ILCDirectory.Data.Models
{
    public class Address
    {
        public int? AddressId { get; set; }
        public bool IsPermanent { get; set; }
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [StringLength(255)]
        public string AddressLine2 { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(50)]
        public string StateProvince { get; set; }
        [StringLength(12)]
        public string PostalCode { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string? ContactPersonName { get; set; }
        [StringLength(50)]
        public string? ContactPersonPhone { get; set; }
        [StringLength(2000)]
        public string Notes { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
        [StringLength(255)]
        public string CubicleNumber { get; set; }
        public bool IncludeInDirectory {  get; set; }
        public bool IsVerified { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName {  get; set; }
        public int DDDId { get; set; }

    }
}
