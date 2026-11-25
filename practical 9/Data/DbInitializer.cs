using PlacementDashboard.Models;

namespace PlacementDashboard.Data
{
    public static class DbInitializer
    {
        public static void Initialize(PlacementDbContext context)
        {
            context.Database.EnsureCreated();

            // Refresh data with new student and company dummy records
            context.Placements.RemoveRange(context.Placements);
            context.Students.RemoveRange(context.Students);
            context.Companies.RemoveRange(context.Companies);
            context.SaveChanges();

            var students = new List<Student>
            {
                new Student { StudentName = "Ananya Sharma", Email = "ananya.sharma@college.edu", Department = "Computer Science", CGPA = 9.20, GraduationYear = 2026 },
                new Student { StudentName = "Rahul Verma", Email = "rahul.verma@college.edu", Department = "Information Technology", CGPA = 8.45, GraduationYear = 2026 },
                new Student { StudentName = "Pooja Kulkarni", Email = "pooja.kulkarni@college.edu", Department = "Computer Science", CGPA = 8.90, GraduationYear = 2026 },
                new Student { StudentName = "Siddharth Nair", Email = "sid.nair@college.edu", Department = "Electronics & Comm", CGPA = 7.60, GraduationYear = 2026 },
                new Student { StudentName = "Sneha Roy", Email = "sneha.roy@college.edu", Department = "Information Technology", CGPA = 9.05, GraduationYear = 2026 },
                new Student { StudentName = "Vikram Singh", Email = "vikram.singh@college.edu", Department = "Mechanical Engg", CGPA = 7.20, GraduationYear = 2026 },
                new Student { StudentName = "Divya Reddy", Email = "divya.reddy@college.edu", Department = "Computer Science", CGPA = 8.75, GraduationYear = 2026 },
                new Student { StudentName = "Amit Patel", Email = "amit.patel@college.edu", Department = "Electronics & Comm", CGPA = 6.95, GraduationYear = 2026 },
                new Student { StudentName = "Meera Iyer", Email = "meera.iyer@college.edu", Department = "Data Science", CGPA = 8.60, GraduationYear = 2026 },
                new Student { StudentName = "Karthik Sundaram", Email = "karthik.s@college.edu", Department = "Information Technology", CGPA = 7.80, GraduationYear = 2026 }
            };

            var companies = new List<Company>
            {
                new Company { CompanyName = "Amazon", JobRole = "Cloud Support Associate", Package = 1450000 },
                new Company { CompanyName = "Deloitte", JobRole = "Analyst - Tech Consulting", Package = 760000 },
                new Company { CompanyName = "Oracle", JobRole = "Associate Applications Developer", Package = 1100000 },
                new Company { CompanyName = "Capgemini", JobRole = "Software Analyst", Package = 550000 },
                new Company { CompanyName = "Cognizant", JobRole = "Programmer Analyst", Package = 480000 },
                new Company { CompanyName = "L&T Infotech", JobRole = "Graduate Engineer Trainee", Package = 520000 }
            };

            context.Students.AddRange(students);
            context.Companies.AddRange(companies);
            context.SaveChanges();

            // Placements for placed students
            context.Placements.AddRange(
                new Placement { StudentID = students[0].StudentID, CompanyID = companies[0].CompanyID, PlacementDate = new DateTime(2026, 2, 10) }, // Ananya -> Amazon
                new Placement { StudentID = students[1].StudentID, CompanyID = companies[1].CompanyID, PlacementDate = new DateTime(2026, 2, 15) }, // Rahul -> Deloitte
                new Placement { StudentID = students[2].StudentID, CompanyID = companies[2].CompanyID, PlacementDate = new DateTime(2026, 2, 20) }, // Pooja -> Oracle
                new Placement { StudentID = students[4].StudentID, CompanyID = companies[0].CompanyID, PlacementDate = new DateTime(2026, 2, 25) }, // Sneha -> Amazon
                new Placement { StudentID = students[6].StudentID, CompanyID = companies[3].CompanyID, PlacementDate = new DateTime(2026, 3, 2) },  // Divya -> Capgemini
                new Placement { StudentID = students[9].StudentID, CompanyID = companies[4].CompanyID, PlacementDate = new DateTime(2026, 3, 10) }  // Karthik -> Cognizant
            );

            context.SaveChanges();
        }
    }
}
