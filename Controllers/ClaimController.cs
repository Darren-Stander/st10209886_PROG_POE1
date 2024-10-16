using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace st10209886_PROG_POE1.Controllers
{
    public class ClaimController : Controller
    {
        private readonly ClaimContext _context;

        // Inject ClaimContext (DbContext) through the constructor
        public ClaimController(ClaimContext context)
        {
            _context = context;
        }

        // GET: Claims/Submit
        public IActionResult Submit()
        {
            return View();
        }

        // POST: Claims/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim)
        {
            if (ModelState.IsValid)
            {
                claim.Status = "Pending"; // Automatically set to Pending when submitted
                _context.Add(claim); // Adds the new claim to the DbContext
                await _context.SaveChangesAsync(); // Saves the new claim to the database
                return RedirectToAction(nameof(Coordinators)); // Redirect to the list of claims
            }

            // If the model is invalid, log the errors (optional)
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                System.Diagnostics.Debug.WriteLine(error.ErrorMessage); // Log errors for debugging
            }

            // Return the form view with validation errors
            return View(claim);
        }

        // GET: Claims/History
        public async Task<IActionResult> History()
        {
            // Return all claims
            var allClaims = await _context.Claims.ToListAsync();
            return View(allClaims);
        }

        // GET: Claim/Coordinators - View pending claims
        public async Task<IActionResult> Coordinators()
        {
            // Get all pending claims from the database
            var pendingClaims = await _context.Claims.Where(c => c.Status == "Pending").ToListAsync();
            return View(pendingClaims);
        }

        // POST: Claim/Approve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Approved"; // Set the claim status to Approved
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            return RedirectToAction(nameof(Coordinators));
        }

        // POST: Claim/Reject
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected"; // Set the claim status to Rejected
                await _context.SaveChangesAsync(); // Save changes to the database
            }
            return RedirectToAction(nameof(Coordinators));
        }
    }
}
