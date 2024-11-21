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

        // Constructor to inject ClaimContext (DbContext)
        public ClaimController(ClaimContext context)
        {
            _context = context;
        }

        // GET: Claim/Submit
        public IActionResult Submit()
        {
            return View();
        }

        // POST: Claim/Submit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Claim claim, IFormFile supportingDocument)
        {
            if (ModelState.IsValid)
            {
                // Handle supporting document upload
                if (supportingDocument != null && supportingDocument.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await supportingDocument.CopyToAsync(memoryStream);
                        var claimFile = new ClaimFile
                        {
                            FileName = supportingDocument.FileName,
                            FileData = memoryStream.ToArray(), // Save file data as byte array
                        };

                        // Associate the file with the claim
                        claim.ClaimFiles.Add(claimFile);
                    }
                }

                // Set the default status for the claim
                claim.Status = "Pending";

                // Save the claim to the database
                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                // Redirect back to the Submit page after successful save
                return RedirectToAction(nameof(Submit));
            }

            // If validation fails, return the user to the same form with error messages
            return View(claim);
        }
    }
}
