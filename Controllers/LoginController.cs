using employee_management_agile.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace employee_management_agile.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginDbContext _context;

        public LoginController(LoginDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginPage()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginPage(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.LoginsTable.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    var httpclaims = new List<Claim>()
                    {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, "Employee") // Add the user's role as a claim
                    };
                    var identity = new ClaimsIdentity(httpclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true, // Set to true if you want the cookie to persist across sessions
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30) // Set the cookie expiration time
                        });
                    // Authentication successful, redirect to the desired page
                    return RedirectToAction("GetAllEmployees", "Emp");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogoutPage()
        {

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Sign out the user
            // Implement logout logic here (e.g., clear authentication cookies)
            return RedirectToAction("LoginPage", "Login");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterPage()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterPage(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.LoginsTable.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return View(model);
                }
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

                _context.LoginsTable.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("LoginPage", "Login");
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}