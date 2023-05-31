using ILCDirectory.Pages.Account;

namespace ILCDirectory.Authentication
{
    public interface IUserManager
    {
        Task SignIn(HttpContext httpContext, LoginModel loginInfo, bool isPersistent = false);
        Task SignOut(HttpContext httpContext);
    }
}
