namespace RiskDashBoard.Models.ViewModels
{
    public class PhaseViewModel
    {
        public int PhaseId { get; set; }
        public bool IsCurrentPhase { get; set; }
        public int ProjectId { get; set; }
        public string RiskTypeDecission { get; set; }
        public int NumberLowRisk { get; set; }
        public int NumberMediumRisk { get; set; }
        public int NumberHighRisk { get; set; }
        public int NumberBlockerRisk { get; set; }
    }
}
