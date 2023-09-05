using ILCDirectory.Models;
using Microsoft.AspNetCore.Authentication;
using Sustainsys.Saml2.AspNetCore2;

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
            return await OnPost();
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> OnPost()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = "/"
            };

            // On application initiated signout, it's the application's responsibility
            // to both terminate the local session and issue a remote signout.
            await _userManager.SignOut(this.HttpContext);
            return SignOut(props, Saml2Defaults.Scheme, "cookie");
        }

        //public async Task<IActionResult> OnPost()
        //{
        //    try
        //    {
        //        await _userManager.SignOut(this.HttpContext);
        //        return RedirectToPage("/Account/Login");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error logging out");
        //        throw;
        //    }
        //}   
    }
}
