using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Data;
using System.Security.Claims;
using ILCDirectory.Models;
using ILCDirectory.Pages.Account;

namespace ILCDirectory.Authentication
{
    public class UserManager : IUserManager
    {
        private readonly IConfiguration _cfg;
        public UserManager(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public async Task SignIn(HttpContext httpContext, LoginInfo loginInfo, bool isPersistent = false)
        {
            var successUser = loginInfo; // change this

            ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(successUser), CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(LoginInfo user) // change this model to match our authentication model
        {

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.EmailAddress));
            claims.Add(new Claim(ClaimTypes.Role, "BugHunter"));
            return claims;
        }
    }
}
