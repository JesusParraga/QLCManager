using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models.ViewModels
{
    public class RiskViewModel
    {
        [Key]
        public int RiskId { get; set; }
        [Required]
        [DisplayName("Title")]
        public string? RiskName { get; set; }
        [Required]
        [DisplayName("Description")]
        public string? RiskDescription { get; set; }
        [Required]
        [DisplayName("Probability")]
        public int RiskProbability { get; set; }
        [Required]
        [DisplayName("Impact")]
        public int RiskImpact { get; set; }
        [Required]
        public int PhaseId { get; set; }
        [Required]
        public bool Resolved { get; set; }

        public int ProjectId { get; set; }
    }
}
