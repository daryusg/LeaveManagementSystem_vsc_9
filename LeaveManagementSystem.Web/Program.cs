using LeaveManagementSystem.Application;
using Constants = LeaveManagementSystem.Data.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
DataServicesRegistration.AddDataServices(builder.Services, builder.Configuration); //cip...174. moved 2 entries to LeaveManagementSystem.Data.DataServicesRegistration.AddDataServices
ApplicationServicesRegistration.AddApplicationServices(builder.Services); //cip..173. moved automapper + 5 builder.Services to LeaveManagementSystem.Application.ApplicationServicesRegistration.AddApplicationServices

builder.Services.AddAuthorization(options => {
    options.AddPolicy(Constants.Policies.cAdminSupervisorOnly, policy => {
        policy.RequireRole(Constants.Roles.cAdministrator, Constants.Roles.cSupervisor); //either or
        //policy.RequireRole(Constants.Roles.cAdministrator); //and
        //policy.RequireRole(Constants.Roles.cSupervisor); //and
    });
}); //cip...165

builder.Services.AddHttpContextAccessor(); //cip...127

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true) //cip...107. (default user) IdentityUser->ApplicationUser
//cip...108 Register.cshtml.cs if (_userManager.Options.SignIn.RequireConfirmedAccount) -> options.SignIn.RequireConfirmedAccount = true
    .AddRoles<IdentityRole>() //cip...107
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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
