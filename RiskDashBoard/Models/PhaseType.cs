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
            1 => StaticInfo.ProjectPhasesEnum.EXPLORATION.ToString(),
            2 => StaticInfo.ProjectPhasesEnum.VALUATION.ToString(),
            3 => StaticInfo.ProjectPhasesEnum.FOUNDATIONS.ToString(),
            4 => StaticInfo.ProjectPhasesEnum.DEVELOPMENT.ToString(),
            5 => StaticInfo.ProjectPhasesEnum.OPERATION.ToString(),
            _ => StaticInfo.ProjectPhasesEnum.NONE.ToString(),
        };

    }
}
