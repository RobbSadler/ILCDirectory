namespace ILCDirectory.Data.Models
{
    public partial class Person
    {
        public int? PersonId { get; set; }
        public string FirstName { get; set; }
        [StringLength(25)]
        public string MiddleName { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(25)]
        [DisplayFormat(DataFormatString = " ({0})")]
        public string MaidenName { get; set; }
        [StringLength(25)]
        [DisplayFormat(DataFormatString = "({0})")]
        public string NickName { get; set; }
        [StringLength(1)]
        public string MaritalStatus { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("DOBirth mm/dd/yyyy")]
        public DateTime? DateOfBirth { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(255)]
        public string Comment { get; set; }
        public Suffix? Suffix { get; set; }
        public Title? Title { get; set; }
        [StringLength(255)]
        public bool DeleteFlag { get; set; }
        [StringLength(255)]
        public string Position { get; set; }
        [StringLength(255)]
        public string SupervisorName { get; set; }
        [StringLength(255)]
        public string SupervisorNotes { get; set; }
        [StringLength(255)]
        public string WoCode { get; set; }
        public int? WorkgroupCode { get; set; }
        [StringLength(255)]
        public string FieldOfService { get; set; }
        [StringLength(255)]
        public string ClassificationCode { get; set; }
        [StringLength(255)]
        public string LanguagesSpoken { get; set; }
        public bool IncludeInDirectory { get; set; } // master switch - if false, no directory info is displayed even if address, phone, etc. are set to true

        [StringLength(2000)]
        public string Notes { get; set; }
        public DateTimeOffset CreateDateTime { get; set; }
        public DateTimeOffset ModifiedDateTime { get; set; }
        public string ModifiedByUserName { get; set; }
        public int DDDId { get; set; }
    }
}
