namespace ILCDirectory.Data.Models
{
    [Table("Classification")]
    public partial class Classification
    {
        [Key]
        public int? ClassificationId { get; set; }
        [StringLength(255)]
        public string ClassificationCode { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public string Notes { get; set; }
        [StringLength(255)]
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }
    }
}
