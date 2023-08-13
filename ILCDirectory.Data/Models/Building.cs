namespace ILCDirectory.Data.Models
{
    public partial class Building
    {
        [Key]
        public int BuildingId { get; set; }
        [StringLength(255)]
        public string BuildingCode { get; set; }
        [StringLength(255)]
        public string BuildingLongDesc { get; set; }
        [StringLength(255)]
        public string BuildingShortDesc { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }

    }
}
