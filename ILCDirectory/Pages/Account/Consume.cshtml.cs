using ILCDirectory.Models;
using Saml;

namespace ILCDirectory.Pages.Account
{
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

        [HttpPost]
        [AllowAnonymous]
        //[Route("/Account/Consume")]
        public IActionResult OnPost()
        {
            // 1. TODO: specify the certificate that your SAML provider gave you
            string samlCertificate = @"-----BEGIN CERTIFICATE-----
MIICFTCCAYKgAwIBAgIQzfcJCkM1YahDtRGYsLphrDAJBgUrDgMCHQUAMCExHzAd
BgNVBAMTFnN0dWJpZHAuc3VzdGFpbnN5cy5jb20wHhcNMTcxMjE0MTE1NDUwWhcN
MzkxMjMxMjM1OTU5WjAhMR8wHQYDVQQDExZzdHViaWRwLnN1c3RhaW5zeXMuY29t
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDSSq8EX46J1yprfaBdh4pWII+/
E7ypHM1NjG7mCwFwbkjq2tpSBuoASrQftbjIKqjVzxtxETw802VJu5CJR4d3Zdy5
jD8NRTesfaQDazX7iiqisfnxmIdDhtJS0lXeBlj4MipoUW6l8Qsjx7ltZSwdfFLy
h+bMqIrwOhMWGs82vQIDAQABo1YwVDBSBgNVHQEESzBJgBCBBNba7KNF5wnXqmYc
ejn6oSMwITEfMB0GA1UEAxMWc3R1YmlkcC5zdXN0YWluc3lzLmNvbYIQzfcJCkM1
YahDtRGYsLphrDAJBgUrDgMCHQUAA4GBAHonBGahlldp7kcN5HGGnvogT8a0nNpM
7GMdKhtzpLO3Uk3HyT3AAIKWiSoEv2n1BTalJ/CY/+te/JZPVGhWVzoi5JYytpj5
gM0O7RH0a3/yUE8S8YLV2h0a2gsdoMvTRTnTm9CnXezCKqhjYjwsmOZtiCIYuFqX
71d/pg5uoJfs
-----END CERTIFICATE-----";

            // 2. Let's read the data - SAML providers usually POST it into the "SAMLResponse" var
            Saml.Response samlResponse = new Response(samlCertificate, Request.Form["SAMLResponse"]);

            // 3. We're done!

            var username = "";

            if (samlResponse.IsValid())
                username = samlResponse.GetNameID();


            /*            if (!ModelState.IsValid)
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
            */




            return Redirect("/Home");
        }

    }
}
