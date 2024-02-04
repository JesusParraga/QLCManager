using Humanizer;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using static RiskDashBoard.Resources.StaticInfo;

namespace RiskDashBoard.Models
{
    public class HistoricPhase
    {
        public int HistoricPhaseId { get; set; }
        public string? PreviousPhaseType { get; set; }
        public string? CurrentPhaseType { get; set; }
        public string? Comments { get; set; }
        public int ProjectId { get; set; }
        public virtual Project? Project { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? Date { get; set; }
        public bool IsBack { get; set; }
        public int DecissionId { get; set; }
        public int ProposalRiskDecission {  get; set; }
        public int PhaseRiskEvaluation { get; set; }
        public int IterationPhaseNumber { get; set; }

        [NotMapped]
        public string HumanizedDate => (DateTime.Now - Date).Value.Humanize(culture: new CultureInfo("en-En"));

        [NotMapped]
        public string DecissionDescription => GetDescriptionAttributeEnum((PhaseDecissionProposalTypeEnum)DecissionId);

        [NotMapped]
        public string DecissionProposalDescription => GetDescriptionAttributeEnum((PhaseDecissionProposalTypeEnum)ProposalRiskDecission);

        [NotMapped]
        public string PhaseRiskEvaluationDescription => ((RiskLevelEvaluationTypeEnum)PhaseRiskEvaluation).ToString();
    }
}
