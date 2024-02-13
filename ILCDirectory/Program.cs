using Microsoft.AspNetCore.Authentication.Cookies;
using Sustainsys.Saml2.AspNetCore2;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using ElmahCore.Mvc;
using ElmahCore.Sql;

DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("Policy1",
            policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
    });

// Add services to the container.
var connectionString = builder.Configuration[Constants.CONFIG_CONNECTION_STRING];

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    });

//builder.Services.AddTransient(m => new UserManager("string here for now"));
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddSingleton<ITokenizeAndSearchRepository, TokenizeAndSearchRepository>();
builder.Services.AddSingleton<IILCDirectoryRepository, ILCDirectoryRepository>();
builder.Services.AddSingleton<IUserManager, UserManager>();

builder.Services.AddElmah<SqlErrorLog>(options =>
{
    options.ConnectionString = builder.Configuration[Constants.CONFIG_CONNECTION_STRING];
    options.OnPermissionCheck = context => context.User.Identity!.IsAuthenticated;
});

builder.Services.AddControllers();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options => 
        options.Conventions.AddPageRoute("/Main/Index", "")
        );

// Use the cookie scheme as default scheme, even for challenge. We need the
// challenge to go through a page under our control to wire up the external login
// callback pattern.
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie", opt =>
    {
        // When challenged, redirect to this path. This captures
        // any page tried as a returnUrl query string parameter.
        opt.LoginPath = "/Account/Login";
    })
    .AddCookie("external")
    .AddSaml2(Saml2Defaults.Scheme, opt =>
    {
        // When Saml2 is done, it should persist the resulting identity
        // in the external cookie.
        opt.SignInScheme = "external";

        // Set up our EntityId, this is our application.
        opt.SPOptions.EntityId = new EntityId("https://localhost:11123/Saml2");

        // Single logout messages should be signed according to the SAML2 standard, so we need
        // to add a certificate for our app to sign logout messages with to enable logout functionality.
        opt.SPOptions.ServiceCertificates.Add(new X509Certificate2("Sustainsys.Saml2.Tests.pfx"));

        // Add an identity provider.
        opt.IdentityProviders.Add(new IdentityProvider(
            // The identityprovider's entity id.
            new EntityId("https://stubidp.sustainsys.com/Metadata"),
            opt.SPOptions)
        {
            // Load config parameters from metadata, using the Entity Id as the metadata address.
            LoadMetadata = true
        });
    });

builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Logging.AddNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
       name: "default",
          pattern: "{controller=Home}/{action=Main}/{id?}");
app.MapRazorPages();
app.UseElmah();

app.Run();
