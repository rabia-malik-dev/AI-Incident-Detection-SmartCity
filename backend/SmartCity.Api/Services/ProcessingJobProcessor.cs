using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Contracts.Ml;
using SmartCity.Api.Data;
using SmartCity.Api.Entities;

namespace SmartCity.Api.Services;

public class ProcessingJobProcessor(IncidentDbContext db, MlAnalysisClient ml, ILogger<ProcessingJobProcessor> logger)
{
    public async Task<bool> ProcessNextPendingAsync(CancellationToken ct)
    {
        var job = await db.ProcessingJobs
            .Include(j => j.Video)
            .ThenInclude(v => v.CameraProfile)
            .Where(j => j.Status == JobStatus.Pending)
            .OrderBy(j => j.CreatedAtUtc)
            .FirstOrDefaultAsync(ct);

        if (job is null)
            return false;

        job.Status = JobStatus.Running;
        job.StartedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);

        try
        {
            if (!File.Exists(job.Video.StoragePath))
                throw new FileNotFoundException("Video file missing on disk.", job.Video.StoragePath);

            var profile = MapCameraProfile(job.Video.CameraProfile);
            var mlResult = await ml.AnalyzeVideoAsync(job.Video.StoragePath, job.Video.FileName, profile, ct);

            var now = DateTime.UtcNow;
            foreach (var item in mlResult.Violations)
            {
                db.Violations.Add(new Violation
                {
                    VideoId = job.VideoId,
                    ProcessingJobId = job.Id,
                    ViolationType = item.ViolationType,
                    Confidence = item.Confidence,
                    DetectedAtUtc = now,
                    FrameIndex = item.FrameIndex,
                    FrameTimeSeconds = item.FrameTimeSeconds,
                    TrackId = item.TrackId,
                    ObjectClass = item.ObjectClass,
                    BboxJson = item.Bbox is null ? null : JsonSerializer.Serialize(item.Bbox),
                    FrameImagePath = item.FrameImagePath,
                });
            }

            job.Status = JobStatus.Completed;
            job.CompletedAtUtc = DateTime.UtcNow;
            logger.LogInformation(
                "Job {JobId} completed: {ViolationCount} violations in {Ms}ms (ML)",
                job.Id,
                mlResult.Violations.Count,
                mlResult.ProcessingTimeMs);
        }
        catch (Exception ex)
        {
            job.Status = JobStatus.Failed;
            job.ErrorMessage = ex.Message.Length > 2000 ? ex.Message[..2000] : ex.Message;
            job.CompletedAtUtc = DateTime.UtcNow;
            logger.LogError(ex, "Job {JobId} failed", job.Id);
        }

        await db.SaveChangesAsync(ct);
        return true;
    }

    private static MlCameraProfilePayload MapCameraProfile(CameraProfile? profile)
    {
        if (profile is null)
            return new MlCameraProfilePayload { AllowedDirectionDeg = 90.0 };

        return new MlCameraProfilePayload
        {
            Name = profile.Name,
            AllowedDirectionDeg = profile.AllowedDirectionDeg ?? 90.0,
            SpeedLimitKmh = profile.SpeedLimitKmh,
            MetersPerPixel = profile.MetersPerPixel,
            RoiPolygonJson = profile.RoiPolygonJson,
        };
    }
}
