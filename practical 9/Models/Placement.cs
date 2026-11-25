
namespace PlacementDashboard.Models
{
    public class Placement
    {
        public int PlacementID { get; set; }

        public int StudentID { get; set; }

        public int CompanyID { get; set; }

        public DateTime PlacementDate { get; set; }

        public Student? Student { get; set; }

        public Company? Company { get; set; }
    }
}
