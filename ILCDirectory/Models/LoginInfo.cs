namespace ILCDirectory.Models
{
    public class LoginInfo
    {
        [Required]
        [Display(Name = "Email address")]
        [StringLength(100)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(100)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
