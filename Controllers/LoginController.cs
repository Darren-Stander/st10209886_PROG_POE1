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
            ViewData["ErrorMessage"] = string.Empty;
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ViewData["ErrorMessage"] = "Email, Password, and Role are required.";
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewData["ErrorMessage"] = "Invalid login attempt.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, role))
                {
                    return role switch
                    {
                        "Lecturer" => RedirectToAction("Submit", "Claim"),
                        "Coordinator" => RedirectToAction("Coordinators", "Claim"),
                        "HR" => RedirectToAction("ClaimStatus", "Claim"),
                        _ => RedirectToAction("Index", "Login")
                    };
                }

                ViewData["ErrorMessage"] = "The selected role does not match the user's assigned role.";
                await _signInManager.SignOutAsync();
                return View();
            }

            ViewData["ErrorMessage"] = "Invalid login attempt.";
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
