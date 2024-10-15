using Microsoft.AspNetCore.Mvc;
using st10209886_PROG_POE1.Models; // Make sure this points to your models folder
using System.Collections.Generic;
using System.Linq;

namespace st10209886_PROG_POE1.Controllers
{
    public class ClaimController : Controller
    {
        // Simulate a data source with a list of claims
        private static List<Claim> claims = new List<Claim>
        {
            new Claim { ClaimId = 1, LecturerNumber = "L001", HoursWorked = 10, HourlyRate = 50, SupportingDocument = "doc1.pdf", Status = "Pending" },
            new Claim { ClaimId = 2, LecturerNumber = "L002", HoursWorked = 8, HourlyRate = 40, SupportingDocument = "doc2.docx", Status = "Pending" }
        };

        // GET: Claims/Submit
        public IActionResult Submit()
        {
            return View();
        }

        // GET: Claims/History
        public IActionResult History()
        {
            return View();
        }

        // GET: Claim/Coordinators - View pending claims
        public IActionResult Coordinators()
        {
            // Get all pending claims
            var pendingClaims = claims.Where(c => c.Status == "Pending").ToList();
            return View(pendingClaims);
        }

        // POST: Claim/Approve
        [HttpPost]
        public IActionResult Approve(int claimId)
        {
            var claim = claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.Status = "Approved"; // Set the claim status to Approved
            }
            return RedirectToAction("Coordinators");
        }

        // POST: Claim/Reject
        [HttpPost]
        public IActionResult Reject(int claimId)
        {
            var claim = claims.FirstOrDefault(c => c.ClaimId == claimId);
            if (claim != null)
            {
                claim.Status = "Rejected"; // Set the claim status to Rejected
            }
            return RedirectToAction("Coordinators");
        }
    }
}
