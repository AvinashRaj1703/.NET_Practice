
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;

namespace SmartCampusPortal.Controllers
{
    public class CampusServicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampusServicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.CampusServices
                .OrderBy(s => s.ServiceName)
                .ToListAsync();

            return View(services);
        }
    }
}
