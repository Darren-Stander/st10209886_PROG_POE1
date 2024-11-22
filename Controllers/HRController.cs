using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using st10209886_PROG_POE1.Models;
using System.Linq;
using System.Threading.Tasks;

namespace st10209886_PROG_POE1.Controllers
{
    public class HRController : Controller
    {
        private readonly ClaimContext _context;

        public HRController(ClaimContext context)
        {
            _context = context;
        }

        // Action to display approved claims
        public async Task<IActionResult> ApprovedClaims()
        {
            var approvedClaims = await _context.Claims
                .Include(c => c.Lecturer) // Include Lecturer details
                .Where(c => c.Status == "Approved")
                .ToListAsync();

            return View(approvedClaims);
        }

        // List all lecturers
        public async Task<IActionResult> ManageLecturers()
        {
            var lecturers = await _context.Lecturers.ToListAsync();
            return View(lecturers);
        }

        // Add Lecturer (GET)
        public IActionResult AddLecturer()
        {
            return View();
        }

        // Add Lecturer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLecturer(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Add(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageLecturers));
            }
            return View(lecturer);
        }

        // Edit Lecturer (GET)
        public async Task<IActionResult> EditLecturer(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // Edit Lecturer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecturer(Lecturer lecturer)
        {
            if (ModelState.IsValid)
            {
                _context.Lecturers.Update(lecturer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageLecturers));
            }
            return View(lecturer);
        }

        // Delete Lecturer (GET)
        public async Task<IActionResult> DeleteLecturer(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // Delete Lecturer (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLecturerConfirmed(int id)
        {
            var lecturer = await _context.Lecturers.FindAsync(id);
            if (lecturer != null)
            {
                _context.Lecturers.Remove(lecturer);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ManageLecturers));
        }

    }
}
