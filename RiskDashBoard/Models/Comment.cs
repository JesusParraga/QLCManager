using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace RiskDashBoard.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastUpdatedAt { get; set; }
        public string? UserComment {  get; set; }
        public int RiskId { get; set; }
        public virtual Risk? Risk{ get; set; }

        [NotMapped]
        public string TimeLapse => (DateTime.Now - LastUpdatedAt).Value.Humanize(culture:new CultureInfo("en-En"));
    }
}
