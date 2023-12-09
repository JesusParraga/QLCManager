using Microsoft.EntityFrameworkCore;
using RiskDashBoard.Models;

namespace RiskDashBoard.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Phase> Phase { get; set; } = null!;
        public DbSet<Risk> Risks { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<HistoricPhase> HistoricPhases { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasMany(x => x.Users).WithMany(x => x.Projects).UsingEntity(x => x.ToTable("UserProject"));
            modelBuilder.Entity<Phase>().HasMany(x => x.Risks).WithMany(x => x.Phases).UsingEntity(x => x.ToTable("PhaseRisk"));
            modelBuilder.Entity<Risk>().HasMany(x => x.Tags).WithMany(x => x.Risks).UsingEntity(x => x.ToTable("RiskTag"));
            modelBuilder.Entity<Phase>().HasOne(x => x.Project).WithMany(x => x.Phases).HasForeignKey(x => x.ProjectId).HasConstraintName("FK_ProjectPhase");
            modelBuilder.Entity<HistoricPhase>().HasOne(x => x.Phase).WithMany(x => x.HistoricPhases).HasForeignKey(x => x.PhaseId).HasConstraintName("FK_HistoricPhasePhase");



            base.OnModelCreating(modelBuilder);
        }
    }
}
