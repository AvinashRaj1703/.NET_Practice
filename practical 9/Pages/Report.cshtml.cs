
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlacementDashboard.Data;
using PlacementDashboard.Models;

namespace PlacementDashboard.Pages
{
    public class ReportModel : PageModel
    {
        private readonly PlacementDbContext _context;

        public ReportModel(PlacementDbContext context)
        {
            _context = context;
        }

        public int TotalStudents { get; set; }
        public int PlacedStudents { get; set; }
        public int NotPlacedStudents { get; set; }
        public double PlacementPercentage { get; set; }

        public List<StudentReportItem> StudentRecords { get; set; } = new();
        public List<Company> CompanyRecords { get; set; } = new();

        public void OnGet()
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

            StudentRecords = _context.Students
                .OrderBy(s => s.StudentID)
                .Select(s => new StudentReportItem
                {
                    StudentID = s.StudentID,
                    StudentName = s.StudentName,
                    Department = s.Department,
                    CGPA = s.CGPA,
                    CompanyName = _context.Placements
                        .Where(p => p.StudentID == s.StudentID)
                        .Select(p => p.Company!.CompanyName)
                        .FirstOrDefault(),
                    JobRole = _context.Placements
                        .Where(p => p.StudentID == s.StudentID)
                        .Select(p => p.Company!.JobRole)
                        .FirstOrDefault(),
                    Package = _context.Placements
                        .Where(p => p.StudentID == s.StudentID)
                        .Select(p => (double?)p.Company!.Package)
                        .FirstOrDefault(),
                    PlacementDate = _context.Placements
                        .Where(p => p.StudentID == s.StudentID)
                        .Select(p => (DateTime?)p.PlacementDate)
                        .FirstOrDefault()
                })
                .ToList();

            CompanyRecords = _context.Companies
                .OrderBy(c => c.CompanyName)
                .ToList();
        }
    }

    public class StudentReportItem
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; } = "";
        public string Department { get; set; } = "";
        public double CGPA { get; set; }
        public string? CompanyName { get; set; }
        public string? JobRole { get; set; }
        public double? Package { get; set; }
        public DateTime? PlacementDate { get; set; }
    }
}
