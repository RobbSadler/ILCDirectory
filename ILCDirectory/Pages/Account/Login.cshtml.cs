using ILCDirectory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly UserManager _userManager;

        [Required]
        [Display(Name = "Email address")]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public LoginModel(UserManager userManager)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                //authenticate
                await _userManager.SignIn(this.HttpContext, this);
                return RedirectToAction("Index", "Main", null);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("summary", ex.Message);
                return Page();
            }
        }
    }
}
