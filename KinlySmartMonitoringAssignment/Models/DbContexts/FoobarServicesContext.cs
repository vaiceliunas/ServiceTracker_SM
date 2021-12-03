using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RestApi.Models;

#nullable disable

namespace KinlySmartMonitoringAssignment.Models
{
    public partial class FoobarServicesContext : DbContext
    {
        public FoobarServicesContext()
        {
        }

        public FoobarServicesContext(DbContextOptions<FoobarServicesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Label> Labels { get; set; }
        public virtual DbSet<Service> Services { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.LabelKey)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LabelValue)
                    .IsRequired()
                    .HasMaxLength(50);

            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.HasMany(t => t.Labels).WithOne().OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.MaintainerEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
