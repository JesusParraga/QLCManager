using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class Risk
    {
        [Key]
        public int RiskId { get; set; }
        [Required]
        public string? RiskName { get; set;}
        [Required]
        public string? RiskDescription { get; set;}
        [Required]
        public int RiskLevel { get; set; }
        public ICollection<Phase>? Phases { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
