using Humanizer;
using RiskDashBoard.Resources;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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

        [NotMapped]
        public string HumanizedDate => (DateTime.Now - Date).Value.Humanize(culture: new CultureInfo("en-En"));

        [NotMapped]
        public string DecissionDescription => ((RiskTypeEnum)DecissionId).ToString();
    }
}
