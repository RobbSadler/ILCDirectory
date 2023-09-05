using ILCDirectory.Models;
using Microsoft.AspNetCore.Authentication;
using Sustainsys.Saml2.AspNetCore2;

namespace ILCDirectory.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(IUserManager userManager, ILogger<LoginModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet(string returnUrl)
        {
            returnUrl ??= "/Main/Index";
            if (!Url.IsLocalUrl(returnUrl))
            {
                throw new InvalidOperationException("Open redirect protection");
            }

            var props = new AuthenticationProperties
            {
                RedirectUri = "/Account/Consume",
                Items = { { "returnUrl", returnUrl } }
            };

            return Challenge(props, Saml2Defaults.Scheme);
        }

        [BindProperty]
        public LoginInfo LoginInfo { get; set; }
    }
}
