namespace SmartCity.Api.Contracts;

public record RegisterRequest(string Email, string Password, string DisplayName, string Role = "Admin");
public record LoginRequest(string Email, string Password);

public record AuthResponse(string Token, string Email, string DisplayName, string Role, DateTime ExpiresAtUtc);

public record VideoUploadResponse(int VideoId, int ProcessingJobId, string Status, string Message);

public record VideoListItem(
    int Id,
    string FileName,
    string? Title,
    DateTime UploadedAtUtc,
    int? CameraProfileId,
    string LatestJobStatus);

public record ProcessingJobDto(
    int Id,
    int VideoId,
    string Status,
    DateTime? StartedAtUtc,
    DateTime? CompletedAtUtc,
    string? ErrorMessage,
    int ViolationCount);

public record ViolationDto(
    long Id,
    int VideoId,
    string ViolationType,
    double Confidence,
    DateTime DetectedAtUtc,
    int? FrameIndex,
    double? FrameTimeSeconds,
    int? TrackId,
    string? ObjectClass,
    string? SnapshotUrl);

public record CameraProfileDto(
    int Id,
    string Name,
    double? AllowedDirectionDeg,
    double? SpeedLimitKmh,
    double? MetersPerPixel);
