using ILCDirectory.Models;
using ILCDirectory.Pages.Account;

namespace ILCDirectory.Authentication
{
    public interface IUserManager
    {
        Task SignIn(HttpContext httpContext, LoginInfo loginInfo, bool isPersistent = false);
        Task SignOut(HttpContext httpContext);
    }
}
