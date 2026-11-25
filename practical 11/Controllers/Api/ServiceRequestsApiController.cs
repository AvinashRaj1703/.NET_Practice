
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers.Api
{
    [ApiController]
    [Route("api/servicerequests")]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/servicerequests
        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.ServiceRequests
                .Include(r => r.Student)
                .Include(r => r.CampusService)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.RequestId,
                    r.Title,
                    r.Description,
                    r.Status,
                    r.CreatedAt,
                    StudentName = r.Student!.FullName,
                    ServiceName = r.CampusService!.ServiceName
                })
                .ToListAsync();

            return Ok(requests);
        }

        // GET: api/servicerequests/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequest(int id)
        {
            var request = await _context.ServiceRequests
                .Include(r => r.Student)
                .Include(r => r.CampusService)
                .Where(r => r.RequestId == id)
                .Select(r => new
                {
                    r.RequestId,
                    r.Title,
                    r.Description,
                    r.Status,
                    r.CreatedAt,
                    r.StudentId,
                    StudentName = r.Student!.FullName,
                    r.ServiceId,
                    ServiceName = r.CampusService!.ServiceName
                })
                .FirstOrDefaultAsync();

            if (request == null)
                return NotFound(new { message = "Service request not found." });

            return Ok(request);
        }

        // POST: api/servicerequests
        [HttpPost]
        public async Task<IActionResult> CreateRequest(
            [FromBody] CreateServiceRequestDto request)
        {
            var studentExists = await _context.Students
                .AnyAsync(s => s.StudentId == request.StudentId);

            var serviceExists = await _context.CampusServices
                .AnyAsync(s => s.ServiceId == request.ServiceId);

            if (!studentExists)
                return BadRequest(new { message = "Invalid student ID." });

            if (!serviceExists)
                return BadRequest(new { message = "Invalid service ID." });

            var newRequest = new ServiceRequest
            {
                Title = request.Title,
                Description = request.Description,
                StudentId = request.StudentId,
                ServiceId = request.ServiceId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            _context.ServiceRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRequest),
                new { id = newRequest.RequestId },
                new
                {
                    message = "Service request created successfully.",
                    requestId = newRequest.RequestId
                });
        }

        // PUT: api/servicerequests/1/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateRequestStatusDto request)
        {
            var allowedStatuses = new[]
            {
                "Pending",
                "In Progress",
                "Completed",
                "Rejected"
            };

            if (string.IsNullOrWhiteSpace(request.Status) ||
                !allowedStatuses.Contains(request.Status))
            {
                return BadRequest(new
                {
                    message = "Invalid status. Allowed values: Pending, In Progress, Completed, Rejected."
                });
            }

            var serviceRequest = await _context.ServiceRequests
                .FindAsync(id);

            if (serviceRequest == null)
                return NotFound(new { message = "Service request not found." });

            serviceRequest.Status = request.Status;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Request status updated successfully.",
                requestId = id,
                status = serviceRequest.Status
            });
        }

        // DELETE: api/servicerequests/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound(new { message = "Service request not found." });

            _context.ServiceRequests.Remove(request);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Service request deleted successfully." });
        }
    }

    public class CreateServiceRequestDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public int ServiceId { get; set; }
    }

    public class UpdateRequestStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
