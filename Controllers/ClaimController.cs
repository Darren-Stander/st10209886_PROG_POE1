using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;
using System.IO;
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

        /// POST: Claims/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim, IFormFile supportingDocument)
        {
            if (string.IsNullOrWhiteSpace(claim.LecturerNumber) || claim.HoursWorked <= 0 || claim.HourlyRate <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please ensure all fields are filled with valid numeric values.");
                return View(claim);
            }

            if (!string.IsNullOrEmpty(claim.AdditionalNotes) && !System.Text.RegularExpressions.Regex.IsMatch(claim.AdditionalNotes, @"^[a-zA-Z0-9\s]+$"))
            {
                ModelState.AddModelError("AdditionalNotes", "Additional Notes can only contain letters, numbers, and spaces.");
                return View(claim);
            }

            claim.Status = "Pending"; // Automatically set to Pending when submitted

            if (supportingDocument != null && supportingDocument.Length > 0)
            {
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await supportingDocument.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                var claimFile = new ClaimFile
                {
                    FileName = supportingDocument.FileName,
                    FileData = fileData, // Store file data as byte array
                    ClaimId = claim.ClaimId // Associate the file with the claim
                };

                claim.ClaimFiles.Add(claimFile);
            }

            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Coordinators));
        }

        // GET: Claim/History - Display all claims with their files
        public async Task<IActionResult> History()
        {
            var allClaims = await _context.Claims
                .Include(c => c.ClaimFiles) // Include files
                .ToListAsync();

            return View(allClaims);
        }

        // GET: Claim/ClaimStatus - Display all claims with their current status
        public async Task<IActionResult> ClaimStatus()
        {
            // Fetch all claims from the database
            var allClaims = await _context.Claims.ToListAsync();
            return View(allClaims); // Pass the claims to the view
        }

        // GET: Claim/Coordinators - Display all pending claims with their files
        public async Task<IActionResult> Coordinators()
        {
            var pendingClaims = await _context.Claims
                .Include(c => c.ClaimFiles) // Include related files
                .Where(c => c.Status == "Pending").ToListAsync();

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
                claim.Status = "Approved"; // Mark as approved
                await _context.SaveChangesAsync();
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
                claim.Status = "Rejected"; // Mark as rejected
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Coordinators));
        }
    }
}
