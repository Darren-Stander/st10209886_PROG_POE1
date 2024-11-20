using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace st10209886_PROG_POE1.Controllers
{
    [Authorize] // Ensure only authenticated users can access any actions
    public class ClaimController : Controller
    {
        private readonly ClaimContext _context;

        // Inject ClaimContext (DbContext) through the constructor
        public ClaimController(ClaimContext context)
        {
            _context = context;
        }

        // GET: Claims/Submit - Only accessible by Lecturers
        [Authorize(Roles = "Lecturer")]
        public IActionResult Submit()
        {
            return View();
        }

        // POST: Claims/Submit - Only accessible by Lecturers
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Lecturer")]
        public async Task<IActionResult> Submit(Claim claim, IFormFile supportingDocument)
        {
            // Validate required fields (LecturerNumber, HoursWorked, HourlyRate)
            if (string.IsNullOrWhiteSpace(claim.LecturerNumber) || claim.HoursWorked <= 0 || claim.HourlyRate <= 0)
            {
                ModelState.AddModelError(string.Empty, "Please ensure all fields are filled with valid numeric values.");
                return View(claim);
            }

            // Automatically set the status to "Pending" when submitting
            claim.Status = "Pending";

            // Handle the supporting document if provided
            if (supportingDocument != null && supportingDocument.Length > 0)
            {
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await supportingDocument.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                // Create the ClaimFile object and associate it with the claim
                var claimFile = new ClaimFile
                {
                    FileName = supportingDocument.FileName,
                    FileData = fileData,
                };

                claim.ClaimFiles.Add(claimFile);
            }

            // Add the claim to the database and save changes
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();

            // Redirect to the Coordinators page after submission
            return RedirectToAction(nameof(Coordinators));
        }

        // GET: Claim/History - Display claim history for all roles
        public async Task<IActionResult> History()
        {
            var allClaims = await _context.Claims
                .Include(c => c.ClaimFiles)
                .ToListAsync();

            return View(allClaims);
        }

        // GET: Claim/ClaimStatus - Only accessible by HR
        [Authorize(Roles = "HR")]
        public async Task<IActionResult> ClaimStatus()
        {
            // Fetch all claims from the database
            var allClaims = await _context.Claims.ToListAsync();
            return View(allClaims);
        }

        // GET: Claim/Coordinators - Only accessible by Coordinators
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> Coordinators()
        {
            var pendingClaims = await _context.Claims
                .Include(c => c.ClaimFiles)
                .Where(c => c.Status == "Pending")
                .ToListAsync();

            return View(pendingClaims);
        }

        // POST: Claim/Approve - Only accessible by Coordinators
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> Approve(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Approved";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Coordinators));
        }

        // POST: Claim/Reject - Only accessible by Coordinators
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Coordinator")]
        public async Task<IActionResult> Reject(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim != null)
            {
                claim.Status = "Rejected";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Coordinators));
        }

        // Optional: Download supporting documents
        [HttpGet]
        public async Task<IActionResult> DownloadFile(int fileId)
        {
            var file = await _context.ClaimFiles.FindAsync(fileId);
            if (file == null)
                return NotFound();

            return File(file.FileData, "application/octet-stream", file.FileName);
        }
    }
}
