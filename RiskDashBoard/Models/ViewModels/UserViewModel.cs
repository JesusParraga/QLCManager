using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? ConfirmPassword { get; set; }

        public ICollection<Project>? Projects { get; set; }

        public bool ValidateObjectProperties()
        {
            return !string.IsNullOrEmpty(UserName) &&
                !string.IsNullOrEmpty(Email) &&
                !string.IsNullOrEmpty(Password) &&
                !string.IsNullOrEmpty(ConfirmPassword);
		}
    }
}
