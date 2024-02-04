using static RiskDashBoard.Resources.StaticInfo;

namespace RiskDashBoard.Models.ViewModels
{
    public class PhaseViewModel
    {
        public int PhaseId { get; set; }
        public bool IsCurrentPhase { get; set; }
        public int ProjectId { get; set; }
        public string? EvaluationPhase{ get; set; }
        public List<PhaseType> PhaseTypes { get; set; } 
        public int NumberLowRisk { get; set; }
        public int NumberMediumRisk { get; set; }
        public int NumberHighRisk { get; set; }
        public int NumberBlockerRisk { get; set; }
        public bool IsExplorationPhaseChecked { get; set; }
        public bool IsValuationPhaseChecked { get; set; }
        public bool IsFoundationsPhaseChecked { get; set; }
        public bool IsDevelopmentPhaseChecked { get; set; }
        public bool IsOperationPhaseChecked { get; set; }
        public bool IsExplorationPhaseEnabled { get; set; }
        public bool IsValuationPhaseEnabled { get; set; }
        public bool IsFoundationsPhaseEnabled { get; set; }
        public bool IsDevelopmentPhaseEnabled { get; set; }
        public bool IsOperationPhaseEnabled { get; set; }
        public int ProposalRiskDecission { get; set; }

        public string? ProposalRiskDecissionDescription => GetDescriptionAttributeEnum((PhaseDecissionProposalTypeEnum)ProposalRiskDecission);
    }
}
