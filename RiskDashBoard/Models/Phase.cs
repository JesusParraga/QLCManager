using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class Phase
    {
        [Key]
        public int PhaseId { get; set; }
        public bool IsCurrentPhase { get; set; }
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public int RiskTypeDecission { get; set; }
        public ICollection<Risk>? Risks { get; set; }
        public ICollection<HistoricPhase>? HistoricPhases { get; set; }
        public ICollection<PhaseType>? PhaseTypes { get; set; }
    }
}
