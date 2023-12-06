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
        public DbSet<Risk> Risks { get; set; } = null!;
        public DbSet<Tag>  Tags { get; set; } = null!; 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(x => x.Projects).WithMany(x => x.Users).UsingEntity(x => x.ToTable("UserProject"));
            modelBuilder.Entity<Phase>().HasMany(x => x.Risks).WithMany(x => x.Phases).UsingEntity(x => x.ToTable("PhaseRisk"));
            modelBuilder.Entity<Risk>().HasMany(x => x.Tags).WithMany(x => x.Risks).UsingEntity(x => x.ToTable("RiskTag"));
            modelBuilder.Entity<Phase>().HasOne(x => x.Project).WithMany(x => x.Phases).HasForeignKey(x => x.ProjectId).HasConstraintName("FK_ProjectPhase");


            base.OnModelCreating(modelBuilder);
        }
    }
}
