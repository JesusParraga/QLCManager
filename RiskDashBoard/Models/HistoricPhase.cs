namespace RiskDashBoard.Models
{
    public class HistoricPhase
    {
        public int HistoricPhaseId { get; set; }
        public int PreviousPhaseId { get; set; }
        public int CurrentPhaseId { get; set; }
        public string? Comments { get; set; }
        public int PhaseId { get; set; }
        public virtual Phase? Phase { get; set; }
        public ICollection<Risk>? Risks { get; set; }
    }
}
