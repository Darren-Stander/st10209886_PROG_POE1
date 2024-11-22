using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                TempData["ErrorMessage"] = "Email, password, and role are required.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Index");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Index");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, role);
            if (!isInRole)
            {
                TempData["ErrorMessage"] = $"The selected role does not match the user's assigned role.";
                return RedirectToAction("Index");
            }

            // Clear previous session data
            HttpContext.Session.Clear();

            // Sign in the user
            var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                // Store the selected role in session
                HttpContext.Session.SetString("SelectedRole", role);

                // Redirect based on the role
                return role switch
                {
                    "Lecturer" => RedirectToAction("Submit", "Claim"),
                    "Coordinator" => RedirectToAction("Coordinators", "Claim"),
                    "HR" => RedirectToAction("ClaimStatus", "Claim"),
                    _ => RedirectToAction("Index")
                };
            }

            TempData["ErrorMessage"] = "Login failed. Please try again.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Logout()
        {
            // Clear session data
            HttpContext.Session.Clear();

            // Log out the user
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
/////////////////////////////////////////////////END OF FILE/////////////////////////////////////////////////