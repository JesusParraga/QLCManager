﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<PhaseType> PhaseTypes { get; set; } = null!;
        public DbSet<Risk> Risks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<HistoricPhase> HistoricPhases { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasMany(x => x.Users).WithMany(x => x.Projects).UsingEntity(x => x.ToTable("UserProject"));
            modelBuilder.Entity<Phase>().HasMany(x => x.PhaseTypes).WithMany(x => x.Phases).UsingEntity(x => x.ToTable("PhasePhasesType"));
            modelBuilder.Entity<Phase>().HasOne(x => x.Project).WithMany(x => x.Phases).HasForeignKey(x => x.ProjectId).HasConstraintName("FK_ProjectPhase");
            modelBuilder.Entity<Risk>().HasMany(x => x.Tags).WithMany(x => x.Risks).UsingEntity(x => x.ToTable("RiskTag"));
            modelBuilder.Entity<Risk>().HasOne(x => x.Phase).WithMany(x => x.Risks).HasForeignKey(x => x.PhaseId).HasConstraintName("FK_RiskPhase");
            modelBuilder.Entity<Comment>().HasOne(x=> x.Risk).WithMany(x => x.Comments).HasForeignKey(x => x.RiskId).HasConstraintName("FJ_CommentRisk");
            modelBuilder.Entity<PhaseType>().HasMany(x => x.Risks).WithMany(x => x.PhasesType).UsingEntity(x => x.ToTable("PhaseTypeRisk"));
            modelBuilder.Entity<HistoricPhase>().HasOne(x => x.Project).WithMany(x => x.HistoricPhases).HasForeignKey(x => x.ProjectId).HasConstraintName("FK_HistoricProject");



            base.OnModelCreating(modelBuilder);
        }
    }
}
