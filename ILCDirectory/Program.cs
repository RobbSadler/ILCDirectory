using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration[Constants.CONFIG_CONNECTION_STRING];

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

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

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<IAddressRepository, AddressRepository>();
builder.Services.AddSingleton<IBuildingRepository, BuildingRepository>();
//builder.Services.AddSingleton<ICityCodeRepository, CityCodeRepository>();
builder.Services.AddSingleton<IClassificationRepository, ClassificationRepository>();
builder.Services.AddSingleton<IEmailRepository, EmailRepository>();
builder.Services.AddSingleton<IFamilyRepository, FamilyRepository>();
builder.Services.AddSingleton<IMailDeliveryRepository, MailDeliveryRepository>();
builder.Services.AddSingleton<IOtherMailRepository, OtherMailRepository>();
builder.Services.AddSingleton<IPersonRepository, PersonRepository>();
builder.Services.AddSingleton<IVehicleRepository, VehicleRepository>();
builder.Services.AddSingleton<IVisitRepository, VisitRepository>();
builder.Services.AddSingleton<IWoRepository, WoRepository>();
builder.Services.AddSingleton<IWorkgroupRepository, WorkgroupRepository>();
builder.Services.AddSingleton<IUserManager, UserManager>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();

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

app.MapRazorPages();

app.Run();
