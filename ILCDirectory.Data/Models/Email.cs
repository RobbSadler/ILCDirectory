namespace ILCDirectory.Data.Models
{
    public class Email
    {
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [Column(TypeName = "tinyint")]
        public EmailAddressType EmailAddressType { get; set; }
        public bool IncludeInDirectory { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }
    }
}
