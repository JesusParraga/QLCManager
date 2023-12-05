using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class Phase
    {
        [Key]
        public int ProjectPhaseId { get; set; }
        [Required]
        public string? ProjectPhaseName { get; set;}
       
        public int ProjectId { get; set; }
        public Project? project { get; set; }
        public ICollection<Risk>? Risks { get;}
    }
}
