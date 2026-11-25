
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;

namespace SmartCampusPortal.Controllers.Api
{
    [ApiController]
    [Route("api/services")]
    public class ServicesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/services
        [HttpGet]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.CampusServices
                .Select(s => new
                {
                    s.ServiceId,
                    s.ServiceName,
                    s.Description,
                    s.Category
                })
                .ToListAsync();

            return Ok(services);
        }

        // GET: api/services/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var service = await _context.CampusServices
                .Where(s => s.ServiceId == id)
                .Select(s => new
                {
                    s.ServiceId,
                    s.ServiceName,
                    s.Description,
                    s.Category
                })
                .FirstOrDefaultAsync();

            if (service == null)
                return NotFound(new { message = "Campus service not found." });

            return Ok(service);
        }
    }
}
