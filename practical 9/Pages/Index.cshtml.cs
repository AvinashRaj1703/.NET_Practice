
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlacementDashboard.Data;
using PlacementDashboard.Models;

namespace PlacementDashboard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly PlacementDbContext _context;

        public IndexModel(PlacementDbContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; set; }
        public int PlacedStudents { get; set; }
        public int NotPlacedStudents { get; set; }
        public double PlacementPercentage { get; set; }

        public string SearchTerm { get; set; } = "";
        public string SelectedDepartment { get; set; } = "";
        public string PlacementStatus { get; set; } = "All";

        public List<string> Departments { get; set; } = new();
        public List<StudentPlacementViewModel> StudentRecords { get; set; } = new();
        public List<Company> CompanyRecords { get; set; } = new();

        public void OnGet(string? searchTerm, string? selectedDepartment,
                          string? placementStatus)
        {
            TotalStudents = _context.Students.Count();

            PlacedStudents = _context.Placements
                .Select(p => p.StudentID)
                .Distinct()
                .Count();

            NotPlacedStudents = TotalStudents - PlacedStudents;

            PlacementPercentage = TotalStudents > 0
                ? (PlacedStudents * 100.0) / TotalStudents
                : 0;

            SearchTerm = searchTerm ?? "";
            SelectedDepartment = selectedDepartment ?? "";
            PlacementStatus = placementStatus ?? "All";

            Departments = _context.Students
                .Select(s => s.Department)
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            var query = _context.Students.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(s =>
                    s.StudentName.Contains(SearchTerm) ||
                    s.Email.Contains(SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(SelectedDepartment))
            {
                query = query.Where(s =>
                    s.Department == SelectedDepartment);
            }

            if (PlacementStatus == "Placed")
            {
                query = query.Where(s =>
                    _context.Placements.Any(p => p.StudentID == s.StudentID));
            }
            else if (PlacementStatus == "NotPlaced")
            {
                query = query.Where(s =>
                    !_context.Placements.Any(p => p.StudentID == s.StudentID));
            }

            StudentRecords = query
                .OrderBy(s => s.StudentID)
                .Select(s => new StudentPlacementViewModel
                {
                    StudentID = s.StudentID,
                    StudentName = s.StudentName,
                    Email = s.Email,
                    Department = s.Department,
                    CGPA = s.CGPA,
                    CompanyName = _context.Placements
                        .Where(p => p.StudentID == s.StudentID)
                        .Select(p => p.Company!.CompanyName)
                        .FirstOrDefault(),
                    JobRole = _context.Placements
                        .Where(p => p.StudentID == s.StudentID)
                        .Select(p => p.Company!.JobRole)
                        .FirstOrDefault()
                })
                .ToList();

            CompanyRecords = _context.Companies.ToList();
        }
    }

    public class StudentPlacementViewModel
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Department { get; set; } = "";
        public double CGPA { get; set; }
        public string? CompanyName { get; set; }
        public string? JobRole { get; set; }
    }
}
