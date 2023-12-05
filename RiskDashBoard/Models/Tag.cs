using System.ComponentModel.DataAnnotations;

namespace RiskDashBoard.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        [Required]
        public string? TagName { get; set; }
        [Required]
        public string? Color { get; set; }

        public ICollection<Risk>? Risks { get; set; }
    }
}
