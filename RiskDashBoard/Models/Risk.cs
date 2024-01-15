using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static RiskDashBoard.Resources.StaticInfo;

namespace RiskDashBoard.Models
{
    public class Risk
    {
        [Key]
        public int RiskId { get; set; }
        [Required]
        public string? RiskName { get; set; }
        [Required]
        public string? RiskDescription { get; set; }
        [Required]
        public int RiskProbability { get; set; }
        [Required]
        public int RiskImpact {  get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Phase>? Phases { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<PhaseType>? PhasesType { get; set; }

        [NotMapped]
        public string PhasesName { get => GetPhasesName(); }
        
        [NotMapped]
        public int RiskLevel { get => GetRiskLevel(); }

        [NotMapped]
        public string RiskLevelName { get => GetRiskLevelName(); }

        private string GetPhasesName(){
            string phasesName = string.Empty;

            if (PhasesType != null && PhasesType.Count > 0) {
                var phasestypeIds = (from p in PhasesType select p.PhaseTypeName ).ToList();

                if (phasestypeIds.Any())
                {
                    phasesName = string.Join(", ", GetPhasesName(phasestypeIds)); 
                }
            }

            return phasesName;
        }

        private List<string> GetPhasesName(List<int> phasestypeIds)
        {
            var phasesName = new List<string>();
            foreach (var phasestype in phasestypeIds) {
                phasesName.Add(GetPhaseName(Convert.ToInt32(phasestype)));
            }
            return phasesName;
        }

        private string GetPhaseName(int phaseTypeId)
        {
            return phaseTypeId switch
            {
                1 => ProjectPhasesEnum.EXPLORATION.ToString(),
                2 => ProjectPhasesEnum.VALUATION.ToString(),
                3 => ProjectPhasesEnum.FOUNDATIONS.ToString(),
                4 => ProjectPhasesEnum.DEVELOPMENT.ToString(),
                5 => ProjectPhasesEnum.OPERATION.ToString(),
                _ => ProjectPhasesEnum.NONE.ToString(),
            };
        }

        private int GetRiskLevel()
        {
            if(RiskProbability == (int)RiskProbabilityEnum.RARE) {
                if (RiskImpact == (int)RiskImpactEnum.INSIGNIFICAT) return (int)RiskLevelEnum.LOW;
                if (RiskImpact == (int)RiskImpactEnum.MINOR) return (int)RiskLevelEnum.LOW;
                if (RiskImpact == (int)RiskImpactEnum.MODERATE) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.ELDERLY) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.CATASTROHIC) return (int)RiskLevelEnum.MEDIUM;
            }

            if (RiskProbability == (int)RiskProbabilityEnum.UNLIKELY)
            {
                if (RiskImpact == (int)RiskImpactEnum.INSIGNIFICAT) return (int)RiskLevelEnum.LOW;
                if (RiskImpact == (int)RiskImpactEnum.MINOR) return (int)RiskLevelEnum.LOW;
                if (RiskImpact == (int)RiskImpactEnum.MODERATE) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.ELDERLY) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.CATASTROHIC) return (int)RiskLevelEnum.HIGH;
            }

            if (RiskProbability == (int)RiskProbabilityEnum.POSSIBLE)
            {
                if (RiskImpact == (int)RiskImpactEnum.INSIGNIFICAT) return (int)RiskLevelEnum.LOW;
                if (RiskImpact == (int)RiskImpactEnum.MINOR) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.MODERATE) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.ELDERLY) return (int)RiskLevelEnum.HIGH;
                if (RiskImpact == (int)RiskImpactEnum.CATASTROHIC) return (int)RiskLevelEnum.HIGH;
            }

            if (RiskProbability == (int)RiskProbabilityEnum.LIKELY)
            {
                if (RiskImpact == (int)RiskImpactEnum.INSIGNIFICAT) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.MINOR) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.MODERATE) return (int)RiskLevelEnum.HIGH;
                if (RiskImpact == (int)RiskImpactEnum.ELDERLY) return (int)RiskLevelEnum.HIGH;
                if (RiskImpact == (int)RiskImpactEnum.CATASTROHIC) return (int)RiskLevelEnum.BLOCKER;
            }

            if (RiskProbability == (int)RiskProbabilityEnum.CERTAIN)
            {
                if (RiskImpact == (int)RiskImpactEnum.INSIGNIFICAT) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.MINOR) return (int)RiskLevelEnum.MEDIUM;
                if (RiskImpact == (int)RiskImpactEnum.MODERATE) return (int)RiskLevelEnum.HIGH;
                if (RiskImpact == (int)RiskImpactEnum.ELDERLY) return (int)RiskLevelEnum.BLOCKER;
                if (RiskImpact == (int)RiskImpactEnum.CATASTROHIC) return (int)RiskLevelEnum.BLOCKER;
            }

            return (int)RiskLevelEnum.LOW;
        }
    
        private string GetRiskLevelName()
        {
            return RiskLevel switch
            {
                (int)RiskLevelEnum.LOW => RiskLevelEnum.LOW.ToString(),
                (int)RiskLevelEnum.MEDIUM => RiskLevelEnum.MEDIUM.ToString(),
                (int)RiskLevelEnum.HIGH => RiskLevelEnum.HIGH.ToString(),
                (int)RiskLevelEnum.BLOCKER => RiskLevelEnum.BLOCKER.ToString(),
                _ => RiskLevelEnum.LOW.ToString(),
            };
        }
    }
}
