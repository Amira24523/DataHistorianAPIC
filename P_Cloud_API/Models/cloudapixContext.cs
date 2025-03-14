using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace P_Cloud_API.Models
{
    public partial class cloudapixContext : DbContext
    {
        public cloudapixContext()
        {
        }

        public cloudapixContext(DbContextOptions<cloudapixContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Areas { get; set; } = null!;
        public virtual DbSet<ControlModule> ControlModules { get; set; } = null!;
        public virtual DbSet<ControlModuleInfo> ControlModuleInfos { get; set; } = null!;
        public virtual DbSet<EditType> EditTypes { get; set; } = null!;
        public virtual DbSet<Enterprise> Enterprises { get; set; } = null!;
        public virtual DbSet<EquipmentModule> EquipmentModules { get; set; } = null!;
        public virtual DbSet<PhysicalUnit> PhysicalUnits { get; set; } = null!;
        public virtual DbSet<ProcessCell> ProcessCells { get; set; } = null!;
        public virtual DbSet<ProcessData> ProcessData { get; set; } = null!;
        public virtual DbSet<Site> Sites { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=CloudApiDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.ToTable("Area");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SiteId).HasColumnName("SiteID");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Areas)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK__Area__SiteID__40F9A68C");
            });

            modelBuilder.Entity<ControlModule>(entity =>
            {
                entity.ToTable("ControlModule");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ControlModuleInfoId).HasColumnName("ControllModuleInfoID");

                entity.Property(e => e.EquipmentModuleId).HasColumnName("EquipmentModuleID");

                entity.Property(e => e.SuperiorControlModuleId).HasColumnName("SuperiorControlModuleID");

                entity.HasOne(d => d.ControlModuleInfo)
                    .WithMany(p => p.ControlModules)
                    .HasForeignKey(d => d.ControlModuleInfoId)
                    .HasConstraintName("FK__ControlMo__Contr__5F7E2DAC");

                entity.HasOne(d => d.EquipmentModule)
                    .WithMany(p => p.ControlModules)
                    .HasForeignKey(d => d.EquipmentModuleId)
                    .HasConstraintName("FK__ControlMo__Equip__4D5F7D71");

                entity.HasOne(d => d.SuperiorControlModule)
                    .WithMany(p => p.InverseSuperiorControlModule)
                    .HasForeignKey(d => d.SuperiorControlModuleId)
                    .HasConstraintName("FK__ControlMo__Super__4E53A1AA");
            });

            modelBuilder.Entity<ControlModuleInfo>(entity =>
            {
                entity.ToTable("ControlModuleInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ControlModuleId).HasColumnName("ControlModuleID");

                entity.Property(e => e.EditTypeId).HasColumnName("EditTypeID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.PhysicalUnitId).HasColumnName("PhysicalUnitID");

                entity.Property(e => e.RangeLowerEnd).HasColumnType("decimal(10, 3)");

                entity.Property(e => e.RangeUpperEnd).HasColumnType("decimal(10, 3)");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.Tolerance).HasColumnType("decimal(5, 3)");

                entity.Property(e => e.Type).HasMaxLength(255);

                entity.Property(e => e.UserIp)
                    .HasMaxLength(255)
                    .HasColumnName("UserIP");

                entity.Property(e => e.Username).HasMaxLength(255);

                entity.HasOne(d => d.ControlModule)
                    .WithMany(p => p.ControlModuleInfos)
                    .HasForeignKey(d => d.ControlModuleId)
                    .HasConstraintName("FK__ControlMo__Contr__56E8E7AB");

                entity.HasOne(d => d.EditType)
                    .WithMany(p => p.ControlModuleInfos)
                    .HasForeignKey(d => d.EditTypeId)
                    .HasConstraintName("FK__ControlMo__EditT__57DD0BE4");

                entity.HasOne(d => d.PhysicalUnit)
                    .WithMany(p => p.ControlModuleInfos)
                    .HasForeignKey(d => d.PhysicalUnitId)
                    .HasConstraintName("FK__ControlMo__Physi__59C55456");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ControlModuleInfos)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__ControlMo__Statu__58D1301D");
            });

            modelBuilder.Entity<EditType>(entity =>
            {
                entity.ToTable("EditType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Enterprise>(entity =>
            {
                entity.ToTable("Enterprise");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<EquipmentModule>(entity =>
            {
                entity.ToTable("EquipmentModule");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.SuperiorEquipmentModuleId).HasColumnName("SuperiorEquipmentModuleID");

                entity.Property(e => e.UnitId).HasColumnName("UnitID");

                entity.HasOne(d => d.SuperiorEquipmentModule)
                    .WithMany(p => p.InverseSuperiorEquipmentModule)
                    .HasForeignKey(d => d.SuperiorEquipmentModuleId)
                    .HasConstraintName("FK__Equipment__Super__4A8310C6");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.EquipmentModules)
                    .HasForeignKey(d => d.UnitId)
                    .HasConstraintName("FK__Equipment__UnitI__498EEC8D");
            });

            modelBuilder.Entity<PhysicalUnit>(entity =>
            {
                entity.ToTable("PhysicalUnit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Fullname).HasMaxLength(255);

                entity.Property(e => e.ShortName).HasMaxLength(255);
            });

            modelBuilder.Entity<ProcessCell>(entity =>
            {
                entity.ToTable("ProcessCell");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AreaId).HasColumnName("AreaID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.ProcessCells)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK__ProcessCe__AreaI__43D61337");
            });

            modelBuilder.Entity<ProcessData>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ControlModuleId).HasColumnName("ControlModuleID");

                entity.Property(e => e.CurrentValue).HasColumnType("decimal(10, 3)");

                entity.Property(e => e.EditTypeId).HasColumnName("EditTypeID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.StatusMessage).HasMaxLength(255);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserIp)
                    .HasMaxLength(255)
                    .HasColumnName("UserIP");

                entity.Property(e => e.Username).HasMaxLength(255);

                entity.HasOne(d => d.ControlModule)
                    .WithMany(p => p.ProcessData)
                    .HasForeignKey(d => d.ControlModuleId)
                    .HasConstraintName("FK__ProcessDa__Contr__5CA1C101");

                entity.HasOne(d => d.EditType)
                    .WithMany(p => p.ProcessData)
                    .HasForeignKey(d => d.EditTypeId)
                    .HasConstraintName("FK__ProcessDa__EditT__5D95E53A");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProcessData)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK__ProcessDa__Statu__5E8A0973");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.ToTable("Site");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EnterpriseId).HasColumnName("EnterpriseID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Enterprise)
                    .WithMany(p => p.Sites)
                    .HasForeignKey(d => d.EnterpriseId)
                    .HasConstraintName("FK__Site__Enterprise__3E1D39E1");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.ProcessCellId).HasColumnName("ProcessCellID");

                entity.HasOne(d => d.ProcessCell)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.ProcessCellId)
                    .HasConstraintName("FK__Unit__ProcessCel__46B27FE2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
