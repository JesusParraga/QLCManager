using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        [DisplayName("Title")]
        public string? ProjectName { get; set; }
        [Required]
        [DisplayName("Project status")]
        public int ProjectStatus { get; set; }
        [Required]
        [DisplayName("Create date")]
        public DateTime ProjectCreateDate { get; set; }
        [Required]
        [DisplayName("Last update date")]
        public DateTime ProjectLastUpdateDate { get; set;}
        public ICollection<User>? Users { get; set; }
        public ICollection<Phase>? Phases { get; set; }
        public ICollection<HistoricPhase>? HistoricPhases { get; set; }
    }
}
