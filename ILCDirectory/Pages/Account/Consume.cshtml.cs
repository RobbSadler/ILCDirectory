using ILCDirectory.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Sustainsys.Saml2;
using System.Security.Claims;

namespace ILCDirectory.Pages.Account
{
    [AllowAnonymous]
    public class ConsumeModel : PageModel
    {
        private readonly IUserManager _userManager;
        //private readonly IRoleRepository _roleRepo;
        private readonly ILogger<LoginModel> _logger;

        public ConsumeModel(IUserManager userManager, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<RedirectResult> OnGet()
        {
            // Get the identity exactly as received from the Saml2 Idp.
            var external = await HttpContext.AuthenticateAsync("external");

            if (!external.Succeeded)
            {
                // We should really not be able to get here without an external
                // identity, but in a production scenario some better error handling
                // is adviced.
                throw new InvalidOperationException();
            }

            // Create a new set of claims based on information in the external identity.
            var claims = new List<Claim>
            {
                new Claim("sub", external.Principal!.FindFirstValue(ClaimTypes.NameIdentifier)),
            };

            // This works based on attributes from the StubIdp Tolvan Tolvansson user.
            var givenName = external.Principal!.FindFirstValue("Subject_GivenName");
            var surName = external.Principal!.FindFirstValue("Subject_Surname");

            var name = (givenName + " " + surName).Trim();

            if (!string.IsNullOrEmpty(name))
            {
                claims.Add(new Claim("name", name));
            }

            // The logout nameidentifier and session index claims are required to be able to
            // initiate a Saml2 logout.
            var logoutNameId = external.Principal!.FindFirst(Saml2ClaimTypes.LogoutNameIdentifier);
            var sessionIndex = external.Principal.FindFirst(Saml2ClaimTypes.SessionIndex);

            if (logoutNameId != null && sessionIndex != null)
            {
                claims.Add(logoutNameId);
                claims.Add(sessionIndex);
            }

            foreach (var role in external.Principal!.FindAll(ClaimTypes.Role))
            {
                // Don't just accept roles from the Idp, pick what roles the Idp
                // is allowed to send and translate it to our app role claim type and
                // app role name.
                if (role.Value == "Administrator")
                {
                    claims.Add(new Claim("role", "admin"));
                }
            }

            // Create an identity and principal that is in the right format for our application.
            // Always use the four param version of the ClaimsIdentity constructor
            var identity = new ClaimsIdentity(claims, "Saml2", "name", "role");
            var principal = new ClaimsPrincipal(identity);

            // Sign in to create a session in our application.
            await HttpContext.SignInAsync(principal);
            // Now we are done with the external identity, call signout on it to remove cookie.
            await HttpContext.SignOutAsync("external");

            try
            {
                // sign in the user and get roles
                await _userManager.SignIn(this.HttpContext, new LoginInfo { EmailAddress = "robb@robbsadler.com" });

                // redirect to the destination url that was put in the props by login.cshtml.cs
                return Redirect(external.Properties!.Items["returnUrl"]!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user");
                ModelState.AddModelError("summary", ex.Message);
                return Redirect("/Home");
            }

        }

        //        public IActionResult OnPost()
        //        {
        //            // 1. TODO: specify the certificate that your SAML provider gave you
        //            string samlCertificate = @"-----BEGIN CERTIFICATE-----
        //MIICFTCCAYKgAwIBAgIQzfcJCkM1YahDtRGYsLphrDAJBgUrDgMCHQUAMCExHzAd
        //BgNVBAMTFnN0dWJpZHAuc3VzdGFpbnN5cy5jb20wHhcNMTcxMjE0MTE1NDUwWhcN
        //MzkxMjMxMjM1OTU5WjAhMR8wHQYDVQQDExZzdHViaWRwLnN1c3RhaW5zeXMuY29t
        //MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDSSq8EX46J1yprfaBdh4pWII+/
        //E7ypHM1NjG7mCwFwbkjq2tpSBuoASrQftbjIKqjVzxtxETw802VJu5CJR4d3Zdy5
        //jD8NRTesfaQDazX7iiqisfnxmIdDhtJS0lXeBlj4MipoUW6l8Qsjx7ltZSwdfFLy
        //h+bMqIrwOhMWGs82vQIDAQABo1YwVDBSBgNVHQEESzBJgBCBBNba7KNF5wnXqmYc
        //ejn6oSMwITEfMB0GA1UEAxMWc3R1YmlkcC5zdXN0YWluc3lzLmNvbYIQzfcJCkM1
        //YahDtRGYsLphrDAJBgUrDgMCHQUAA4GBAHonBGahlldp7kcN5HGGnvogT8a0nNpM
        //7GMdKhtzpLO3Uk3HyT3AAIKWiSoEv2n1BTalJ/CY/+te/JZPVGhWVzoi5JYytpj5
        //gM0O7RH0a3/yUE8S8YLV2h0a2gsdoMvTRTnTm9CnXezCKqhjYjwsmOZtiCIYuFqX
        //71d/pg5uoJfs
        //-----END CERTIFICATE-----";

        //            // 2. Let's read the data - SAML providers usually POST it into the "SAMLResponse" var
        //            Saml.Response samlResponse = new Response(samlCertificate, Request.Form["SAMLResponse"]);

        //            // 3. We're done!

        //            var username = "";

        //            if (samlResponse.IsValid())
        //                username = samlResponse.GetNameID();


        //            /*            if (!ModelState.IsValid)
        //                            return Page();

        //                        try
        //                        {
        //                            //authenticate
        //                            await _userManager.SignIn(this.HttpContext, LoginInfo);
        //                            return RedirectToPage("/Main/Index");
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _logger.LogError(ex, "Error logging in user {0}", LoginInfo.EmailAddress);
        //                            ModelState.AddModelError("summary", ex.Message);
        //                            return Page();
        //                        }
        //            */




        //            return Redirect("/Home");
        //        }

    }
}
