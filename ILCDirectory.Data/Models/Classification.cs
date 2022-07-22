namespace ILCDirectory.Data.Models
{
    [Table("Classification")]
    public partial class Classification
    {
        [Key]
        public int ClassificationId { get; set; }
        [StringLength(255)]
        public string ClassificationCode { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public string AuditTrail { get; set; }
        [StringLength(255)]
        public DateTimeOffset CreateDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string ModifiedByUser { get; set; }
        public int DDDId { get; set; }
    }
}
