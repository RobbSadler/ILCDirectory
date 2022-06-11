using DapperGenericRepository;

namespace ILCDirectory.Data.Models
{
    public partial class Person : GenericRepository<Person>
    {
        IConfiguration _configuration;
        public Person(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Person")
        {
            _configuration = configuration;
        }

        public int PersonId { get; set; }
        [DataType(DataType.Date)]
        
        [DisplayName("DOBirth mm/dd/yyyy")]
        public DateTime? Birthdate { get; set; }
        [StringLength(255)]
        public string? Comment { get; set; }
        [StringLength(25)]
        public string FirstName { get; set; }
        [StringLength(1)]
        public string? Gender { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(25)]
        [DisplayFormat(DataFormatString = " ({0})")]
        public string? MaidenName { get; set; }
        [StringLength(1)]
        public string? MaritalStatus { get; set; }
        [StringLength(25)]
        public string? MiddleName { get; set; }
        [StringLength(25)]
        [DisplayFormat(DataFormatString = "({0})")]
        public string? NickName { get; set; }
        public int? SpouseId { get; set; }
        [StringLength(50)]
        public string? Suffix { get; set; }
        [StringLength(50)]
        public string? Title { get; set; }
        [StringLength(255)]
        public bool? DeleteFlag { get; set; }
        [StringLength(255)]
        public string? StatusCode { get; set; } // ??????????????
        [StringLength(255)]
        public string? LanguagesSpoken { get; set; }
        [StringLength(2000)]
        public string? AuditTrail { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DirectoryCorrectionForm { get; set; }
        [StringLength(120)]
        public string? DirCorrFormNote { get; set; }
    }
}
