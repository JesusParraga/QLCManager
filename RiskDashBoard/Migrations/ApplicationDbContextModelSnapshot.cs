﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RiskDashBoard.Context;

#nullable disable

namespace RiskDashBoard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PhasePhaseType", b =>
                {
                    b.Property<int>("PhaseTypesPhaseTypeId")
                        .HasColumnType("int");

                    b.Property<int>("PhasesPhaseId")
                        .HasColumnType("int");

                    b.HasKey("PhaseTypesPhaseTypeId", "PhasesPhaseId");

                    b.HasIndex("PhasesPhaseId");

                    b.ToTable("PhasePhasesType", (string)null);
                });

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

            modelBuilder.Entity("PhaseTypeRisk", b =>
                {
                    b.Property<int>("PhasesTypePhaseTypeId")
                        .HasColumnType("int");

                    b.Property<int>("RisksRiskId")
                        .HasColumnType("int");

                    b.HasKey("PhasesTypePhaseTypeId", "RisksRiskId");

                    b.HasIndex("RisksRiskId");

                    b.ToTable("PhaseTypeRisk", (string)null);
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

            modelBuilder.Entity("RiskDashBoard.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("RiskId")
                        .HasColumnType("int");

                    b.Property<string>("UserComment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentId");

                    b.HasIndex("RiskId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("RiskDashBoard.Models.HistoricPhase", b =>
                {
                    b.Property<int>("HistoricPhaseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HistoricPhaseId"));

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrentPhaseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsBack")
                        .HasColumnType("bit");

                    b.Property<string>("PreviousPhaseType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HistoricPhaseId");

                    b.HasIndex("ProjectId");

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

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("RiskTypeDecission")
                        .HasColumnType("int");

                    b.HasKey("PhaseId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Phase");
                });

            modelBuilder.Entity("RiskDashBoard.Models.PhaseType", b =>
                {
                    b.Property<int>("PhaseTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PhaseTypeId"));

                    b.Property<int>("PhaseTypeName")
                        .HasColumnType("int");

                    b.HasKey("PhaseTypeId");

                    b.ToTable("PhaseTypes");
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

                    b.Property<string>("RiskDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RiskLevel")
                        .HasColumnType("int");

                    b.Property<string>("RiskName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RiskId");

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

            modelBuilder.Entity("PhasePhaseType", b =>
                {
                    b.HasOne("RiskDashBoard.Models.PhaseType", null)
                        .WithMany()
                        .HasForeignKey("PhaseTypesPhaseTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RiskDashBoard.Models.Phase", null)
                        .WithMany()
                        .HasForeignKey("PhasesPhaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("PhaseTypeRisk", b =>
                {
                    b.HasOne("RiskDashBoard.Models.PhaseType", null)
                        .WithMany()
                        .HasForeignKey("PhasesTypePhaseTypeId")
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

            modelBuilder.Entity("RiskDashBoard.Models.Comment", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Risk", "Risk")
                        .WithMany("Comments")
                        .HasForeignKey("RiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FJ_CommentRisk");

                    b.Navigation("Risk");
                });

            modelBuilder.Entity("RiskDashBoard.Models.HistoricPhase", b =>
                {
                    b.HasOne("RiskDashBoard.Models.Project", "Project")
                        .WithMany("HistoricPhases")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_HistoricProject");

                    b.Navigation("Project");
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

            modelBuilder.Entity("RiskDashBoard.Models.Project", b =>
                {
                    b.Navigation("HistoricPhases");

                    b.Navigation("Phases");
                });

            modelBuilder.Entity("RiskDashBoard.Models.Risk", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
