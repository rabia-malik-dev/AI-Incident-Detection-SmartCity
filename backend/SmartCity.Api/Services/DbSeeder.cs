using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartCity.Api.Data;
using SmartCity.Api.Entities;
using SmartCity.Api.Options;

namespace SmartCity.Api.Services;

public class DbSeeder(IncidentDbContext db, IOptions<SeedOptions> seedOptions, ILogger<DbSeeder> logger)
{
    public async Task SeedAsync(CancellationToken ct)
    {
        var seed = seedOptions.Value;
        if (!seed.Enabled)
            return;

        if (!await db.Users.AnyAsync(ct))
        {
            db.Users.Add(new User
            {
                Email = seed.AdminEmail.ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(seed.AdminPassword),
                DisplayName = seed.AdminDisplayName,
                Role = "Admin",
                CreatedAtUtc = DateTime.UtcNow,
            });
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Seeded admin user {Email}", seed.AdminEmail);
        }

        if (!await db.CameraProfiles.AnyAsync(ct))
        {
            db.CameraProfiles.Add(new CameraProfile
            {
                Name = "Default one-way lane (dev)",
                AllowedDirectionDeg = 90,
                SpeedLimitKmh = 60,
                MetersPerPixel = 0.05,
                RoiPolygonJson = "[[0,0],[1920,0],[1920,1080],[0,1080]]",
                CreatedAtUtc = DateTime.UtcNow,
            });
            await db.SaveChangesAsync(ct);
            logger.LogInformation("Seeded default camera profile.");
        }
    }
}
