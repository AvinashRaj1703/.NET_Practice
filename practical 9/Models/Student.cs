namespace PlacementDashboard.Models
{
    public class Student
    {
        public int StudentID { get; set; }

        public string StudentName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Department { get; set; } = "";

        public double CGPA { get; set; }

        public int GraduationYear { get; set; }
    }
}