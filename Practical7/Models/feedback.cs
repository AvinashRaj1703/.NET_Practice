using System.ComponentModel.DataAnnotations;

namespace WebApplication7.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your feedback")]
        public string Message { get; set; }
    }
}