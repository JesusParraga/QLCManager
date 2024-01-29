using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models.ViewModels
{
    public class UserProjectViewModel
    {
        public int projectId { get; set; }
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

        public bool projectAsigned { get; set; }
    }
}
