#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExtremeIroningTool.Utilitary_classes.DataBaseClasses
{
    public partial class ExtremeIroningDatabaseContext : DbContext
    {
        public ExtremeIroningDatabaseContext()
        {
        }

        public ExtremeIroningDatabaseContext(DbContextOptions<ExtremeIroningDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Armies> Armies { get; set; }
        public virtual DbSet<ArmyGroups> ArmyGroups { get; set; }
        public virtual DbSet<Battalions> Battalions { get; set; }
        public virtual DbSet<ContentsOfArmies> ContentsOfArmies { get; set; }
        public virtual DbSet<ContentsOfArmyGroups> ContentsOfArmyGroups { get; set; }
        public virtual DbSet<ContentsOfDivisions> ContentsOfDivisions { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<Divisions> Divisions { get; set; }
        public virtual DbSet<Generals> Generals { get; set; }
        public virtual DbSet<Modifiers> Modifiers { get; set; }
        public virtual DbSet<PathsToArmyGroupIcons> PathsToArmyGroupIcons { get; set; }
        public virtual DbSet<PathsToArmyIcons> PathsToArmyIcons { get; set; }
        public virtual DbSet<PathsToDivisionIcons> PathsToDivisionIcons { get; set; }
        public virtual DbSet<PathsToModifiersIcons> PathsToModifiersIcons { get; set; }
        public virtual DbSet<TerrainTypes> TerrainTypes { get; set; }
        public virtual DbSet<Tracks> Tracks { get; set; }
        public virtual DbSet<UnitSpecificAttackAdjusters> UnitSpecificAttackAdjusters { get; set; }
        public virtual DbSet<UnitSpecificDefenseAdjusters> UnitSpecificDefenseAdjusters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //sensitive information lmao
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=XOJIODUJIHUK\\MSQL_SERVER_V1;" +
                "Trusted_Connection=Yes;" +
                "DataBase=ExtremeIroningDatabase;" +
                "Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Battalions>(entity =>
            {
                entity.HasKey(e => e.BattalionName)
                    .HasName("PK_Batallions");

                entity.Property(e => e.Type).IsFixedLength();
            });

            modelBuilder.Entity<ContentsOfArmies>(entity =>
            {
                entity.HasOne(d => d.ArmyNameNavigation)
                    .WithMany(p => p.ContentsOfArmies)
                    .HasForeignKey(d => d.ArmyName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contents of armies_Armies");

                entity.HasOne(d => d.DivisionNameNavigation)
                    .WithMany(p => p.ContentsOfArmies)
                    .HasForeignKey(d => d.DivisionName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contents of armies_Divisions");
            });

            modelBuilder.Entity<ContentsOfArmyGroups>(entity =>
            {
                entity.HasOne(d => d.ArmyGroupNavigation)
                    .WithMany(p => p.ContentsOfArmyGroups)
                    .HasForeignKey(d => d.ArmyGroup)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contents of army groups_Army groups");

                entity.HasOne(d => d.ArmyNameNavigation)
                    .WithOne(p => p.ContentsOfArmyGroups)
                    .HasForeignKey<ContentsOfArmyGroups>(d => d.ArmyName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contents of army groups_Armies");
            });

            modelBuilder.Entity<ContentsOfDivisions>(entity =>
            {
                entity.HasOne(d => d.BattalionNameNavigation)
                    .WithMany(p => p.ContentsOfDivisions)
                    .HasForeignKey(d => d.BattalionName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contents of divisions_Batallions");

                entity.HasOne(d => d.DivisionNameNavigation)
                    .WithMany(p => p.ContentsOfDivisions)
                    .HasForeignKey(d => d.DivisionName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contents of divisions_Divisions");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.Property(e => e.CountryTag).IsFixedLength();
            });

            modelBuilder.Entity<Generals>(entity =>
            {
                entity.Property(e => e.Country).IsFixedLength();

                entity.HasOne(d => d.CountryNavigation)
                    .WithMany(p => p.Generals)
                    .HasForeignKey(d => d.Country)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Generals_Countries");
            });

            modelBuilder.Entity<Modifiers>(entity =>
            {
                entity.Property(e => e.BattalionType).IsFixedLength();
            });

            modelBuilder.Entity<PathsToArmyIcons>(entity =>
            {
                entity.HasKey(e => e.Path)
                    .HasName("PK_Path to Army icons");
            });

            modelBuilder.Entity<UnitSpecificAttackAdjusters>(entity =>
            {
                entity.HasOne(d => d.BattalionNameNavigation)
                    .WithOne(p => p.UnitSpecificAttackAdjusters)
                    .HasForeignKey<UnitSpecificAttackAdjusters>(d => d.BattalionName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("[FK_Unit-specific Attack Batallions");
            });

            modelBuilder.Entity<UnitSpecificDefenseAdjusters>(entity =>
            {
                entity.HasOne(d => d.BattalionNameNavigation)
                    .WithOne(p => p.UnitSpecificDefenseAdjusters)
                    .HasForeignKey<UnitSpecificDefenseAdjusters>(d => d.BattalionName)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unit-specific Defense Adjusters_Batallions");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}