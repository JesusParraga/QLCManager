using System.ComponentModel.DataAnnotations;

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

    }
}
