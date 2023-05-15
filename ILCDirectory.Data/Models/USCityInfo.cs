namespace ILCDirectory.Data.Models
{
    public partial class USCityInfo
    {
        [Key]
        public int USCityInfoId { get; set; }
        [StringLength(5)]
        public string ZipCode { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public string State { get; set; }
        [StringLength(255)]
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser { get; set; }
    }
}
