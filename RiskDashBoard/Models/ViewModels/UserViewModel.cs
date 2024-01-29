using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        [Required]
        [DisplayName("Name")]
        public string? UserName { get; set; }
        [Required]
        [DisplayName("Password")]
        public string? Password { get; set; }
        [Required]
        [DisplayName("Email")]
        public string? Email { get; set; }
        [Required]
        [DisplayName("Confirm password")]
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
