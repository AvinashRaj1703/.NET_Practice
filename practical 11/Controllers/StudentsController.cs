
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Student student)
        {
            if (await _context.Students.AnyAsync(
                s => s.Email == student.Email ||
                     s.EnrollmentNumber == student.EnrollmentNumber))
            {
                ModelState.AddModelError("", 
                    "Email or enrollment number already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Registration successful!";

                return RedirectToAction("Index", "CampusServices");
            }

            return View(student);
        }
    }
}
