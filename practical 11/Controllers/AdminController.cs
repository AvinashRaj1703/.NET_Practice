
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;

namespace SmartCampusPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalStudents = await _context.Students.CountAsync();

            ViewBag.TotalServices = await _context.CampusServices.CountAsync();

            ViewBag.TotalRequests = await _context.ServiceRequests.CountAsync();

            ViewBag.PendingRequests = await _context.ServiceRequests
                .CountAsync(r => r.Status == "Pending");

            ViewBag.InProgressRequests = await _context.ServiceRequests
                .CountAsync(r => r.Status == "In Progress");

            ViewBag.CompletedRequests = await _context.ServiceRequests
                .CountAsync(r => r.Status == "Completed");

            var recentRequests = await _context.ServiceRequests
                .Include(r => r.Student)
                .Include(r => r.CampusService)
                .OrderByDescending(r => r.CreatedAt)
                .Take(5)
                .ToListAsync();

            return View(recentRequests);
        }
    }
}
