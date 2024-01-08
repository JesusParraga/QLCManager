using RiskDashBoard.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RiskDashBoard.Models
{
    public class PhaseType
    {
        [Key]
        public int PhaseTypeId { get; set; }
        [Required]
        public int PhaseTypeName { get; set; }
        public ICollection<Risk>? Risks { get; set; }
        public ICollection<Phase>? Phases { get; set; }

        [NotMapped]
        public string PhaseTypeNameDescription => PhaseTypeName switch
        {
            1 => StaticInfo.ProjectPhases.EXPLORATION.ToString(),
            2 => StaticInfo.ProjectPhases.VALUATION.ToString(),
            3 => StaticInfo.ProjectPhases.FOUNDATIONS.ToString(),
            4 => StaticInfo.ProjectPhases.DEVELOPMENT.ToString(),
            5 => StaticInfo.ProjectPhases.OPERATION.ToString(),
            _ => StaticInfo.ProjectPhases.NONE.ToString(),
        };

    }
}
