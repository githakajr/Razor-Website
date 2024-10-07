using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using RazorWebsite.Models;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace RazorWebsite.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Output validation errors to the console or log them
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View(model);
            }

            // Check if the username already exists
            if (await _context.Users.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            // Check if the email already exists
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            // Check password requirements
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Password is required.");
                return View(model);
            }

            if (model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                return View(model);
            }

            // Create a password hash and salt
            using var hmac = new HMACSHA256();
            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(model.Password))),
                PasswordSalt = Convert.ToBase64String(hmac.Key) // Convert salt (key) to Base64 string
            };

            _context.Users.Add(newUser);

            try
            {
                await _context.SaveChangesAsync(); // Save to the database
                ViewBag.SuccessMessage = "Account created successfully. You can now log in.";
                return RedirectToAction("Login");
            }
            catch (DbUpdateException ex)
            {
                // Log the exception (consider using a logging framework)
                Console.WriteLine($"Error saving user: {ex.InnerException?.Message}");
                ModelState.AddModelError("", "An error occurred while saving the user.");
                return View(model);
            }
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken] // Adds CSRF protection
        public async Task<IActionResult> Login(string username, string password)
        {
            // Check if the provided credentials are valid
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                if (string.IsNullOrEmpty(user.PasswordSalt))
        {
            ViewBag.ErrorMessage = "Invalid user data: password salt is missing.";
            return View();
        }
        // Convert the stored salt back from Base64 to byte[] and use it to hash the input password
        var saltBytes = Convert.FromBase64String(user.PasswordSalt);

        using var hmac = new HMACSHA256(saltBytes); // Use the stored salt (key)
        var hashedPassword = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));

        // Compare the hashed input password with the stored password hash
        if (user.PasswordHash == hashedPassword)
                {
                    // Create claims for the authenticated user





                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, username)
                    };

                    // Create claims identity and principal
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Redirect to the home page
                    return RedirectToAction("Index", "Home");
                }
            }

            // If credentials are invalid, return to login view with an error message
            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken] // Adds CSRF protection
        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to the login page
            return RedirectToAction("Login");
        }

        // Other actions (e.g., ExternalLogin, ExternalLoginCallback)...
    }
}
