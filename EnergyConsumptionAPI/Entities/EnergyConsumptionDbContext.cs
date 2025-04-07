using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnergyConsumptionAPI.Entities
{
    public partial class EnergyConsumptionDbContext : DbContext
    {
        public EnergyConsumptionDbContext()
        {
        }

        public EnergyConsumptionDbContext(DbContextOptions<EnergyConsumptionDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EnergyConsumption> EnergyConsumptions { get; set; } = null!;
        public virtual DbSet<EnergyConsumptionForecast> EnergyConsumptionForecasts { get; set; } = null!;
        public virtual DbSet<WeatherCondition> WeatherConditions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnergyConsumption>(entity =>
            {
                entity.HasKey(e => e.ConsumptionId)
                    .HasName("PK__EnergyCo__E3A1C4375D8418D4");

                entity.ToTable("EnergyConsumption");

                entity.HasIndex(e => e.DateTime, "UQ__EnergyCo__03BE4CA1EA5FEEF5")
                    .IsUnique();

                entity.Property(e => e.ConsumptionId).HasColumnName("ConsumptionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<EnergyConsumptionForecast>(entity =>
            {
                entity.HasKey(e => e.ForecastId)
                    .HasName("PK__EnergyCo__7F27445882DA9D8F");

                entity.ToTable("EnergyConsumptionForecast");

                entity.Property(e => e.ForecastId).HasColumnName("ForecastID");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.PredictedAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Model).HasMaxLength(255);
            });

            modelBuilder.Entity<WeatherCondition>(entity =>
            {
                entity.HasKey(e => e.WeatherId)
                    .HasName("PK__WeatherC__0BF97BD54109710A");

                entity.HasIndex(e => e.DateTime, "UQ__WeatherC__03BE4CA1826AB99D")
                    .IsUnique();

                entity.Property(e => e.WeatherId).HasColumnName("WeatherID");

                entity.Property(e => e.Conditions).HasMaxLength(255);

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Temperature).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.DateTimeNavigation)
                    .WithOne(p => p.WeatherCondition)
                    .HasPrincipalKey<EnergyConsumption>(p => p.DateTime)
                    .HasForeignKey<WeatherCondition>(d => d.DateTime)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weather_Energy");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
