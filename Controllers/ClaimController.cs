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
            if (true)
            {
                claim.Status = "Pending"; // Automatically set to Pending when submitted

                

                // Handle file upload (if there is one)
                if (supportingDocument != null && supportingDocument.Length > 0)
                {
                    // Read the file data into a byte array
                    byte[] fileData;
                    using (var memoryStream = new MemoryStream())
                    {
                        await supportingDocument.CopyToAsync(memoryStream);
                        fileData = memoryStream.ToArray();
                    }

                    // Save the file info in the ClaimFiles table
                    var claimFile = new ClaimFile
                    {
                        FileName = supportingDocument.FileName,
                        FileData = fileData, // Store file data as byte array
                        ClaimId = claim.ClaimId // Associate the file with the claim
                    };

                    // Save the claim to the database first
                    claim.ClaimFiles.Add(claimFile);
                    _context.Claims.Add(claim);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Coordinators));
            }

            return View(claim); // If the model state is invalid, return the form with validation errors
        }


        // GET: Claim/History - Display all claims with their files
        public async Task<IActionResult> History()
        {
            // Return all claims, including the associated files
            var allClaims = await _context.Claims
                .Include(c => c.ClaimFiles) // Include files
                .ToListAsync();

            return View(allClaims);
        }

        // GET: Claim/Coordinators - Display all pending claims with their files
        public async Task<IActionResult> Coordinators()
        {
            // Fetch all pending claims and include the associated files
            var pendingClaims = await _context.Claims
                .Include(c => c.ClaimFiles) // Include related files
                .Where(c => c.Status == "Pending").ToListAsync();

            return View(pendingClaims); // Pass the pending claims to the view
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
            return RedirectToAction(nameof(Coordinators)); // Return to the list
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
            return RedirectToAction(nameof(Coordinators)); // Return to the list
        }
    }
}
