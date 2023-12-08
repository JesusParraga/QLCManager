using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models.ViewModels
{
    public class ProjectViewModel
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string? ProjectName { get; set; }
        [Required]
        public DateTime ProjectCreateDate { get; set; }
        [Required]
        public DateTime ProjectLastUpdateDate { get; set; }
        public ICollection<User>? Users { get; set; }
        public ICollection<Phase>? Phases { get; set; }

        public ICollection<UserProjectViewModel>? UserSystem { get; set; }
    }
}
