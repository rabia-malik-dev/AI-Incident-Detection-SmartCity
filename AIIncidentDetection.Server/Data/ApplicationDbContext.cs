using AIIncidentDetection.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace AIIncidentDetection.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Video> Videos => Set<Video>();
        public DbSet<Violation> Violations => Set<Violation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // relationships
            modelBuilder.Entity<Video>()
                .HasOne(v => v.UploadedBy)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Violation>()
                .HasOne(v => v.Video)
                .WithMany(vd => vd.Violations)
                .HasForeignKey(v => v.VideoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
