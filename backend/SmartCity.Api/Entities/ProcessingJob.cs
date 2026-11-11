namespace SmartCity.Api.Entities;

public static class JobStatus
{
    public const string Pending = "Pending";
    public const string Running = "Running";
    public const string Completed = "Completed";
    public const string Failed = "Failed";
    public const string Cancelled = "Cancelled";
}

public class ProcessingJob
{
    public int Id { get; set; }
    public int VideoId { get; set; }
    public string Status { get; set; } = JobStatus.Pending;
    public DateTime? StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public Video Video { get; set; } = null!;
    public ICollection<Violation> Violations { get; set; } = [];
}
