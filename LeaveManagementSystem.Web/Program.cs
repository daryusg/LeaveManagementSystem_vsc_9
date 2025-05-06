using LeaveManagementSystem.Application;
using Constants = LeaveManagementSystem.Data.Constants;
using Serilog;
using System.Globalization;
using Microsoft.AspNetCore.Localization; //cip...177

var builder = WebApplication.CreateBuilder(args);

//--------------------------------------------------
//01/05/25 chatgpt add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
// Optionally add Azure-specific logging
//--------------------------------------------------

/*
builder.Configuration
    .AddEnvironmentVariables(); //30/05/25 This enables reading from Azure App Settings
*/
//--------------------------------------------------
//03/05/25 chatgpt
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
//--------------------------------------------------

// Add services to the container.
DataServicesRegistration.AddDataServices(builder.Services, builder.Configuration); //cip...174. moved 2 entries to LeaveManagementSystem.Data.DataServicesRegistration.AddDataServices
ApplicationServicesRegistration.AddApplicationServices(builder.Services); //cip..173. moved automapper + 5 builder.Services to LeaveManagementSystem.Application.ApplicationServicesRegistration.AddApplicationServices

builder.Host.UseSerilog((ctx, config) => //cip...177
    config.WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration)
);

builder.Services.AddAuthorization(options => {
    options.AddPolicy(Constants.Policies.cAdminSupervisorOnly, policy => {
        policy.RequireRole(Constants.Roles.cAdministrator, Constants.Roles.cSupervisor); //either or
        //policy.RequireRole(Constants.Roles.cAdministrator); //and
        //policy.RequireRole(Constants.Roles.cSupervisor); //and
    });
}); //cip...165

builder.Services.AddHttpContextAccessor(); //cip...127

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequiredLength = 4; //cip...179
    //options.Password.RequireNonAlphanumeric = false; //cip...179
}) //cip...107. (default user) IdentityUser->ApplicationUser
//cip...108 Register.cshtml.cs if (_userManager.Options.SignIn.RequireConfirmedAccount) -> options.SignIn.RequireConfirmedAccount = true
    .AddRoles<IdentityRole>() //cip...107
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

//01/05/25 chatgpt. Set the default culture to UK (en-GB). needed because of my problems entering a Period yesterday
var supportedCultures = new[] { new CultureInfo("en-GB") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-GB"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    builder.WebHost.UseStaticWebAssets(); //cip...176. 03/04/25 1832 from tw.
    app.UseExceptionHandler("/Home/Error");

    // *** chatgpt 29/04/25 TEMP DEBUG CODE - START*** Only for development/debug
    app.UseDeveloperExceptionPage();
    // *** TEMP DEBUG CODE - END  *** Only for development/debug

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
