using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models.ViewModels
{
    public class RiskViewModel
    {
        [Key]
        public int RiskId { get; set; }
        [Required]
        public string? RiskName { get; set; }
        [Required]
        public string? RiskDescription { get; set; }
        [Required]
        public int RiskLevel { get; set; }
        [Required]
        public int PhaseId { get; set; }

        public int ProjectId { get; set; }
    }
}
