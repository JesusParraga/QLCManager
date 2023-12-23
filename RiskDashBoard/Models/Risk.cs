using System.ComponentModel.DataAnnotations;
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
        public int RiskLevel { get; set; }
        public ICollection<Phase>? Phases { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        public ICollection<PhaseType>? PhasesType { get; set; }

        public string PhasesName { get => GetPhasesName(); }

        private string GetPhasesName(){
            string phasesName = string.Empty;

            if (PhasesType != null && PhasesType.Count > 0) {
                var phasestypeIds = (from p in PhasesType select p.PhaseTypeId ).ToList();

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
                1 => ProjectPhases.EXPLORATION.ToString(),
                2 => ProjectPhases.VALUATION.ToString(),
                3 => ProjectPhases.FOUNDATIONS.ToString(),
                4 => ProjectPhases.DEVELOPMENT.ToString(),
                5 => ProjectPhases.OPERATION.ToString(),
                _ => ProjectPhases.NONE.ToString(),
            };
        }
    }
}
