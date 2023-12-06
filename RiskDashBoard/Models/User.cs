using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password{ get; set; }
        [Required]
        public string? Email { get; set; }

        public ICollection<Project>? Projects { get; set; }
    }
}
