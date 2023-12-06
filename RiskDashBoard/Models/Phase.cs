using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class Phase
    {
        [Key]
        public int PhaseId { get; set; }
        [Required]
        public string? PhaseName { get; set;}
       
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public ICollection<Risk>? Risks { get;}
    }
}
