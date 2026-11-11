namespace SmartCity.Api.Entities;

public class Violation
{
    public long Id { get; set; }
    public int VideoId { get; set; }
    public int? ProcessingJobId { get; set; }
    public string ViolationType { get; set; } = string.Empty;
    public double Confidence { get; set; }
    public DateTime DetectedAtUtc { get; set; }
    public int? FrameIndex { get; set; }
    public double? FrameTimeSeconds { get; set; }
    public int? TrackId { get; set; }
    public string? ObjectClass { get; set; }
    public string? BboxJson { get; set; }
    public string? FrameImagePath { get; set; }
    public string? Severity { get; set; }

    public Video Video { get; set; } = null!;
    public ProcessingJob? ProcessingJob { get; set; }
}
