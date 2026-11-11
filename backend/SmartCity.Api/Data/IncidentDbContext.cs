using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Entities;

namespace SmartCity.Api.Data;

public class IncidentDbContext(DbContextOptions<IncidentDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<CameraProfile> CameraProfiles => Set<CameraProfile>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<ProcessingJob> ProcessingJobs => Set<ProcessingJob>();
    public DbSet<Violation> Violations => Set<Violation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Email).HasMaxLength(256);
            e.Property(x => x.PasswordHash).HasMaxLength(512);
            e.Property(x => x.DisplayName).HasMaxLength(200);
            e.Property(x => x.Role).HasMaxLength(50);
        });

        modelBuilder.Entity<CameraProfile>(e =>
        {
            e.ToTable("CameraProfiles");
            e.Property(x => x.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Video>(e =>
        {
            e.ToTable("Videos");
            e.Property(x => x.Title).HasMaxLength(300);
            e.Property(x => x.FileName).HasMaxLength(500);
            e.Property(x => x.StoragePath).HasMaxLength(1000);
            e.HasIndex(x => new { x.UserId, x.UploadedAtUtc });
        });

        modelBuilder.Entity<ProcessingJob>(e =>
        {
            e.ToTable("ProcessingJobs");
            e.Property(x => x.Status).HasMaxLength(50);
            e.Property(x => x.ErrorMessage).HasMaxLength(2000);
            e.HasIndex(x => x.VideoId);
            e.HasIndex(x => x.Status);
        });

        modelBuilder.Entity<Violation>(e =>
        {
            e.ToTable("Violations");
            e.Property(x => x.ViolationType).HasMaxLength(50);
            e.Property(x => x.ObjectClass).HasMaxLength(100);
            e.Property(x => x.BboxJson).HasMaxLength(500);
            e.Property(x => x.FrameImagePath).HasMaxLength(1000);
            e.Property(x => x.Severity).HasMaxLength(20);
            e.HasIndex(x => x.VideoId);
            e.HasIndex(x => new { x.ViolationType, x.DetectedAtUtc });
        });
    }
}
