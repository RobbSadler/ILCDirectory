namespace ILCDirectory.Data.Models
{
    public partial class CityCode
    {
        [Key]
        public int CityCodeId { get; set; }
        [StringLength(255)]
        public string Code { get; set; }
        [StringLength(255)]
        public string ShortDesc { get; set; }
        [StringLength(255)]
        public string LongDesc { get; set; }
        [StringLength(255)]
        public string AuditTrail { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser { get; set; }
        public int DDDId { get; set; }
    }
}
