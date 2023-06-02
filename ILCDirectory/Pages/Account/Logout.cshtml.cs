using ILCDirectory.Models;

namespace ILCDirectory.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IUserManager _userManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(IUserManager userManager, ILogger<LogoutModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            try
            {
                await _userManager.SignOut(this.HttpContext);
                return RedirectToPage("/Account/Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging out");
                throw;
            }
        }
    }
}
