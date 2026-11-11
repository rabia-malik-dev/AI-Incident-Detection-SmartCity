using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SmartCity.Api.Contracts;
using SmartCity.Api.Data;
using SmartCity.Api.Entities;
using SmartCity.Api.Options;

namespace SmartCity.Api.Services;

public class VideoIngestService(IncidentDbContext db, IOptions<StorageOptions> storageOptions)
{
    private readonly StorageOptions _storage = storageOptions.Value;

    public async Task<VideoUploadResponse> UploadAsync(
        int userId,
        IFormFile file,
        string? title,
        int? cameraProfileId,
        CancellationToken ct)
    {
        if (file.Length <= 0)
            throw new InvalidOperationException("Empty file.");
        if (file.Length > _storage.MaxUploadBytes)
            throw new InvalidOperationException($"File exceeds max size of {_storage.MaxUploadBytes} bytes.");

        if (cameraProfileId.HasValue &&
            !await db.CameraProfiles.AnyAsync(c => c.Id == cameraProfileId.Value, ct))
            throw new InvalidOperationException("Camera profile not found.");

        Directory.CreateDirectory(_storage.UploadsPath);
        var safeName = Path.GetFileName(file.FileName);
        var storedName = $"{Guid.NewGuid():N}{Path.GetExtension(safeName)}";
        var fullPath = Path.Combine(_storage.UploadsPath, storedName);

        await using (var fs = File.Create(fullPath))
            await file.CopyToAsync(fs, ct);

        var video = new Video
        {
            UserId = userId,
            CameraProfileId = cameraProfileId,
            Title = string.IsNullOrWhiteSpace(title) ? null : title.Trim(),
            FileName = safeName,
            StoragePath = fullPath,
            UploadedAtUtc = DateTime.UtcNow,
        };
        db.Videos.Add(video);
        await db.SaveChangesAsync(ct);

        var job = new ProcessingJob
        {
            VideoId = video.Id,
            Status = JobStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow,
        };
        db.ProcessingJobs.Add(job);
        await db.SaveChangesAsync(ct);

        return new VideoUploadResponse(
            video.Id,
            job.Id,
            job.Status,
            "Video uploaded. Processing job queued.");
    }
}
