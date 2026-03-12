using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using employee_management_agile.Models;
using employee_management_agile.Controllers;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LoginDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LoginConnection")));
builder.Services.AddDbContext<EmpDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/LoginPage"; // Set the login page URL
        options.LogoutPath = "/Login/LogoutPage"; // Set the logout page URL
        options.AccessDeniedPath = "/Login/AccessDenied"; // Set the access denied page URL
        options.Cookie.HttpOnly = true; // Set HttpOnly for security
        options.Cookie.IsEssential = true; // Make the authentication cookie essential
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOnly", policy => policy.RequireRole("Manager"));
    options.AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Set HttpOnly for security
    options.Cookie.IsEssential = true; // Make the session cookie essential
});

// builder.Services.AddHttpContextAccessor(); // Register IHttpContextAccessor
// {
//     var serviceProvider = builder.Services.BuildServiceProvider();
//     var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
//     var httpContext = httpContextAccessor.HttpContext;
//     var session = httpContext.Session;
//     var signInManager = serviceProvider.GetRequiredService<SignInManager<IdentityUser>>();
//     var httpclaims = new List<Claim>();
//     builder.Services.AddSingleton(httpclaims);
//     builder.Services.AddSingleton(session);
// }


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
// app.MapControllerRoute();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=LoginPage}/{id?}");
app.Run();
