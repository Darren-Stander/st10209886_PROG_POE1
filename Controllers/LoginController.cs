using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace st10209886_PROG_POE1.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // GET: Login
        public IActionResult Index(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return RedirectToAction("SelectRole");
            }

            ViewBag.Role = role; // Pass the selected role to the view
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ModelState.AddModelError(string.Empty, "Email, Password, and Role are required.");
                return View();
            }

            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null && await _userManager.IsInRoleAsync(user, role))
                {
                    // Redirect based on the selected role
                    return role switch
                    {
                        "Lecturer" => RedirectToAction("Submit", "Claim"),
                        "Coordinator" => RedirectToAction("Coordinators", "Claim"),
                        "HR" => RedirectToAction("ClaimStatus", "Claim"),
                        _ => RedirectToAction("Index", "Login")
                    };
                }

                // If the user's role doesn't match the selected role
                ModelState.AddModelError(string.Empty, "The selected role does not match the user's assigned role.");
                await _signInManager.SignOutAsync(); // Log out the user
            }
            else
            {
                // Handle invalid login attempt
                ModelState.AddModelError(string.Empty, "Invalid login credentials.");
            }

            return View();
        }

        // GET: SelectRole
        public IActionResult SelectRole()
        {
            return View();
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
