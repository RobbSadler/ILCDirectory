using ILCDirectory.Models;

namespace ILCDirectory.Pages.Shared
{
    public class _LoginPartial
    {
    }

    public class AccountController : Controller
    {
        UserManager _userManager;

        public AccountController(UserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public IActionResult LogIn(LoginInfo form)
        {
            if (!ModelState.IsValid)
                return View(form);
            try
            {
                //authenticate
                _userManager.SignIn(this.HttpContext, form);
                return RedirectToAction("Search", "Home", null);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("summary", ex.Message);
                return View(form);
            }
        }
    }
}
