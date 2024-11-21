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
        public IActionResult Index()
        {
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

            // Attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Validate the user's role
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null && await _userManager.IsInRoleAsync(user, role))
                {
                    // Redirect to role-specific pages
                    return role switch
                    {
                        "Lecturer" => RedirectToAction("Submit", "Claim"),
                        "Coordinator" => RedirectToAction("Coordinators", "Claim"),
                        "HR" => RedirectToAction("ClaimStatus", "Claim"),
                        _ => RedirectToAction("Index", "Home")
                    };
                }

                ModelState.AddModelError(string.Empty, "The selected role does not match the user's assigned role.");
                await _signInManager.SignOutAsync(); // Logout the user if the role is invalid
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View();
        }
    }
}
