﻿using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Data;
using System.Security.Claims;
using ILCDirectory.Models;
using ILCDirectory.Pages.Account;

namespace ILCDirectory.Authentication
{
    public class UserManager : IUserManager
    {
        string _connectionString;

        public UserManager() //string connectionString)
        {
            //_connectionString = connectionString;
        }

        public async Task SignIn(HttpContext httpContext, LoginModel loginInfo, bool isPersistent = false)
        {
            //using (var con = new SqlConnection(_connectionString))
            //{
                var successUser = loginInfo; // change this

                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(successUser), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //}
        }

        public async Task SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(LoginModel user) // change this model to match our authentication model
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.EmailAddress));
            claims.Add(new Claim(ClaimTypes.Role, "BugHunter"));
            return claims;
        }
    }
}