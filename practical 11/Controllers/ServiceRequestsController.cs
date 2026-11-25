
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;
using SmartCampusPortal.Models;

namespace SmartCampusPortal.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create(int serviceId)
        {
            var service = await _context.CampusServices.FindAsync(serviceId);

            if (service == null)
                return NotFound();

            await LoadStudents();

            var request = new ServiceRequest { ServiceId = serviceId };

            ViewBag.ServiceName = service.ServiceName;

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Description,StudentId,ServiceId")] ServiceRequest request)
        {
            var serviceExists = await _context.CampusServices
                .AnyAsync(s => s.ServiceId == request.ServiceId);

            var studentExists = await _context.Students
                .AnyAsync(s => s.StudentId == request.StudentId);

            if (!serviceExists)
                ModelState.AddModelError("ServiceId", "Invalid campus service.");

            if (!studentExists)
                ModelState.AddModelError("StudentId", "Please select a registered student.");

            if (ModelState.IsValid)
            {
                request.Status = "Pending";
                request.CreatedAt = DateTime.UtcNow;

                _context.ServiceRequests.Add(request);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Service request submitted successfully!";

                return RedirectToAction(nameof(Index));
            }

            await LoadStudents();

            var selectedService = await _context.CampusServices
                .FindAsync(request.ServiceId);

            ViewBag.ServiceName = selectedService?.ServiceName;

            return View(request);
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.ServiceRequests
                .Include(r => r.Student)
                .Include(r => r.CampusService)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> MyRequests(int? studentId)
        {
            var students = await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewBag.Students = new SelectList(
                students, "StudentId", "FullName", studentId);

            if (studentId == null)
                return View(new List<ServiceRequest>());

            var requests = await _context.ServiceRequests
                .Include(r => r.Student)
                .Include(r => r.CampusService)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return View(requests);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            await LoadStudents();

            ViewBag.Services = new SelectList(
                await _context.CampusServices.OrderBy(s => s.ServiceName).ToListAsync(),
                "ServiceId", "ServiceName", request.ServiceId);

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("RequestId,Title,Description,StudentId,ServiceId,Status,CreatedAt")] ServiceRequest request)
        {
            if (id != request.RequestId)
                return NotFound();

            var studentExists = await _context.Students
                .AnyAsync(s => s.StudentId == request.StudentId);

            var serviceExists = await _context.CampusServices
                .AnyAsync(s => s.ServiceId == request.ServiceId);

            if (!studentExists)
                ModelState.AddModelError("StudentId", "Invalid student.");

            if (!serviceExists)
                ModelState.AddModelError("ServiceId", "Invalid service.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(request);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Service request updated successfully!";

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.ServiceRequests.AnyAsync(r => r.RequestId == id))
                        return NotFound();

                    throw;
                }
            }

            await LoadStudents();

            ViewBag.Services = new SelectList(
                await _context.CampusServices.OrderBy(s => s.ServiceName).ToListAsync(),
                "ServiceId", "ServiceName", request.ServiceId);

            return View(request);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var request = await _context.ServiceRequests
                .Include(r => r.Student)
                .Include(r => r.CampusService)
                .FirstOrDefaultAsync(r => r.RequestId == id);

            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var request = await _context.ServiceRequests.FindAsync(id);

            if (request != null)
            {
                _context.ServiceRequests.Remove(request);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Service request deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // Update request status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var allowedStatuses = new[]
            {
                "Pending",
                "In Progress",
                "Completed",
                "Rejected"
            };

            if (!allowedStatuses.Contains(status))
                return BadRequest("Invalid request status.");

            var request = await _context.ServiceRequests.FindAsync(id);

            if (request == null)
                return NotFound();

            request.Status = status;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Request status updated successfully!";

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadStudents()
        {
            var students = await _context.Students
                .OrderBy(s => s.FullName)
                .ToListAsync();

            ViewBag.Students = new SelectList(
                students, "StudentId", "FullName");
        }
    }
}

