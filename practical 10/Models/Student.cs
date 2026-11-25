using System.ComponentModel.DataAnnotations;

namespace Practical_10.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Course is required")]
        public string Course { get; set; } = string.Empty;

        [Range(16, 60, ErrorMessage = "Age must be between 16 and 60")]
        public int Age { get; set; }
    }
}