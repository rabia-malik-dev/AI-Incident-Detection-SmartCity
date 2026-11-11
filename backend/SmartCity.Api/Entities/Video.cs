namespace SmartCity.Api.Entities;

public class Video
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? CameraProfileId { get; set; }
    public string? Title { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public double? DurationSeconds { get; set; }
    public DateTime UploadedAtUtc { get; set; }

    public User User { get; set; } = null!;
    public CameraProfile? CameraProfile { get; set; }
    public ICollection<ProcessingJob> ProcessingJobs { get; set; } = [];
    public ICollection<Violation> Violations { get; set; } = [];
}
