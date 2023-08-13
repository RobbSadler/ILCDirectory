using ILCDirectory.Models;
using Saml;

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

        public IActionResult OnGet()
        {
            var context = HttpContext;
            var samlEndpoint = "https://stubidp.sustainsys.com/e64887c8-c492-49e3-94d5-08d4471ee265/";

            var request = new AuthRequest(
                "https://localhost:11123", //TODO: put your app's "entity ID" here
                
                
                //"https://localhost:11123/Main" //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
                
                
                
                "https://localhost:11123/Account/Consume" //TODO: put Assertion Consumer URL (where the provider should redirect users after authenticating)
            );

            //now send the user to the SAML provider
            return Redirect(request.GetRedirectUrl(samlEndpoint));
        }



        [BindProperty]
        public LoginInfo LoginInfo { get; set; }

        /*        public async Task<IActionResult> OnPost()
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
                }*/
    }
}
