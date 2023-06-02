using ILCDirectory.Models;

namespace ILCDirectory.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IUserManager _userManager;
        //private readonly IRoleRepository _roleRepo;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IUserManager userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public LoginInfo LoginInfo { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            try
            {
                //authenticate
                await _userManager.SignIn(this.HttpContext, LoginInfo);
                return RedirectToPage("/Main/Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user {0}", LoginInfo.EmailAddress);
                ModelState.AddModelError("summary", ex.Message);
                return Page();
            }
        }
    }
}
