using SmartCampusPortal.Models;

namespace SmartCampusPortal.Data
{
    public static class DataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.CampusServices.Any())
            {
                return;
            }

            var services = new List<CampusService>
            {
                new CampusService
                {
                    ServiceName = "IT & Wi-Fi Support",
                    Description = "Assistance for campus Wi-Fi access, lab computer setup, and student portal credentials.",
                    Category = "Technical"
                },
                new CampusService
                {
                    ServiceName = "Central Library",
                    Description = "Book reservations, renewals, issue of reference material, and digital library queries.",
                    Category = "Academic"
                },
                new CampusService
                {
                    ServiceName = "Hostel Maintenance",
                    Description = "Room electrical repairs, plumbing issues, furniture repair, and sanitation requests.",
                    Category = "Hostel"
                },
                new CampusService
                {
                    ServiceName = "College Transportation",
                    Description = "Bus routes, bus pass renewal, timings, and campus transit inquiries.",
                    Category = "Transport"
                },
                new CampusService
                {
                    ServiceName = "Student Administration",
                    Description = "Bonafide certificate issuance, fee receipt duplicates, and identity card replacements.",
                    Category = "Administration"
                },
                new CampusService
                {
                    ServiceName = "Sports & Gymnasium",
                    Description = "Sports equipment issue, badminton court booking, and gym membership inquiries.",
                    Category = "Sports"
                }
            };

            context.CampusServices.AddRange(services);
            context.SaveChanges();

            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student { FullName = "Aarav Patel", Email = "aarav.patel@college.edu", EnrollmentNumber = "21BCE045", Department = "Computer Engineering" },
                    new Student { FullName = "Keyuri Joshi", Email = "keyuri.joshi@college.edu", EnrollmentNumber = "21BCE102", Department = "Computer Engineering" },
                    new Student { FullName = "Rohan Sharma", Email = "rohan.sharma@college.edu", EnrollmentNumber = "21BIT018", Department = "Information Technology" },
                    new Student { FullName = "Priya Nair", Email = "priya.nair@college.edu", EnrollmentNumber = "22BEC034", Department = "Electronics Engineering" },
                    new Student { FullName = "Harsh Mehta", Email = "harsh.mehta@college.edu", EnrollmentNumber = "22BME012", Department = "Mechanical Engineering" }
                };

                context.Students.AddRange(students);
                context.SaveChanges();

                var requests = new List<ServiceRequest>
                {
                    new ServiceRequest
                    {
                        Title = "Wi-Fi connection not working in Lab 3",
                        Description = "Unable to connect student laptops to the campus academic Wi-Fi network in Computer Lab 3.",
                        Status = "In Progress",
                        CreatedAt = DateTime.UtcNow.AddDays(-2),
                        StudentId = students[0].StudentId,
                        ServiceId = services[0].ServiceId
                    },
                    new ServiceRequest
                    {
                        Title = "Book Renewal Request for Database Systems",
                        Description = "Need to extend return date for Database System Concepts (Silberschatz) by 7 days for exam preparation.",
                        Status = "Completed",
                        CreatedAt = DateTime.UtcNow.AddDays(-5),
                        StudentId = students[1].StudentId,
                        ServiceId = services[1].ServiceId
                    },
                    new ServiceRequest
                    {
                        Title = "Fan regulator broken in Hostel Room B-204",
                        Description = "Ceiling fan regulator is stuck at maximum speed in Hostel Block B, Room 204. Please arrange repair.",
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow.AddHours(-8),
                        StudentId = students[0].StudentId,
                        ServiceId = services[2].ServiceId
                    },
                    new ServiceRequest
                    {
                        Title = "Bonafide Certificate for Railway Concession",
                        Description = "Require a bonafide student certificate for railway concession form submission for semester break.",
                        Status = "Completed",
                        CreatedAt = DateTime.UtcNow.AddDays(-3),
                        StudentId = students[2].StudentId,
                        ServiceId = services[4].ServiceId
                    },
                    new ServiceRequest
                    {
                        Title = "Morning Bus Route 5 Delay",
                        Description = "College bus on Route 5 arrived 25 minutes late at City Center stop this morning.",
                        Status = "Pending",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        StudentId = students[3].StudentId,
                        ServiceId = services[3].ServiceId
                    }
                };

                context.ServiceRequests.AddRange(requests);
                context.SaveChanges();
            }
        }
    }
}
