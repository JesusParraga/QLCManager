﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RiskDashBoard.Context;

#nullable disable

namespace RiskDashBoard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231209073147_PhasesMigration")]
    partial class PhasesMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PhaseRisk", b =>
                {
                    b.Property<int>("PhasesPhaseId")
                        .HasColumnType("int");

                    b.Property<int>("RisksRiskId")
                        .HasColumnType("int");

                    b.HasKey("PhasesPhaseId", "RisksRiskId");

                    b.HasIndex("RisksRiskId");

                    b.ToTable("PhaseRisk", (string)null);
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.Property<int>("ProjectsProjectId")
                        .HasColumnType("int");

                    b.Property<int>("UsersUserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectsProjectId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("UserProject", (string)null);
                });

            modelBuilder.Entity("RiskDashBoard.Models.HistoricPhase", b =>
                {
                    b.Property<int>("HistoricPhaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HistoricPhaseId"));

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CurrentPhaseId")
                        .HasColumnType("int");

                    b.Property<int>("PhaseId")
                        .HasColumnType("int");

                    b.Property<int>("PreviousPhaseId")
                        .HasColumnType("int");

                    b.HasKey("HistoricPhaseId");

                    b.HasIndex("PhaseId");

                    b.ToTable("HistoricPhases");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Phase", b =>
                {
                    b.Property<int>("PhaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseId"));

                    b.Property<bool>("IsCurrentPhase")
                        .HasColumnType("bit");

                    b.Property<int>("PhaseTypeId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("PhaseId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Phase");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<DateTime>("ProjectCreateDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ProjectLastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Risk", b =>
                {
                    b.Property<int>("RiskId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RiskId"));

                    b.Property<int?>("HistoricPhaseId")
                        .HasColumnType("int");

                    b.Property<string>("RiskDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RiskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RiskType")
                        .HasColumnType("int");

                    b.HasKey("RiskId");

                    b.HasIndex("HistoricPhaseId");

                    b.ToTable("Risks");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TagId"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TagId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("RiskDashBoard.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RiskTag", b =>
                {
                    b.Property<int>("RisksRiskId")
                        .HasColumnType("int");

                    b.Property<int>("TagsTagId")
                        .HasColumnType("int");

                    b.HasKey("RisksRiskId", "TagsTagId");

                    b.HasIndex("TagsTagId");

                    b.ToTable("RiskTag", (string)null);
                });

            modelBuilder.Entity("PhaseRisk", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Phase", null)
                        .WithMany()
                        .HasForeignKey("PhasesPhaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiskDashBoard.Models.Risk", null)
                        .WithMany()
                        .HasForeignKey("RisksRiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjectUser", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Project", null)
                        .WithMany()
                        .HasForeignKey("ProjectsProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiskDashBoard.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RiskDashBoard.Models.HistoricPhase", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Phase", "Phase")
                        .WithMany("HistoricPhases")
                        .HasForeignKey("PhaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_HistoricPhasePhase");

                    b.Navigation("Phase");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Phase", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Project", "Project")
                        .WithMany("Phases")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_ProjectPhase");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Risk", b =>
                {
                    b.HasOne("RiskDashBoard.Models.HistoricPhase", null)
                        .WithMany("Risks")
                        .HasForeignKey("HistoricPhaseId");
                });

            modelBuilder.Entity("RiskTag", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Risk", null)
                        .WithMany()
                        .HasForeignKey("RisksRiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiskDashBoard.Models.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RiskDashBoard.Models.HistoricPhase", b =>
                {
                    b.Navigation("Risks");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Phase", b =>
                {
                    b.Navigation("HistoricPhases");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Project", b =>
                {
                    b.Navigation("Phases");
                });
#pragma warning restore 612, 618
        }
    }
}
