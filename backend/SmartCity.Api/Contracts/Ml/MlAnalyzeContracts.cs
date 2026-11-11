using System.Text.Json.Serialization;

namespace SmartCity.Api.Contracts.Ml;

public sealed class MlCameraProfilePayload
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("allowed_direction_deg")]
    public double AllowedDirectionDeg { get; set; } = 90;

    [JsonPropertyName("speed_limit_kmh")]
    public double? SpeedLimitKmh { get; set; }

    [JsonPropertyName("meters_per_pixel")]
    public double? MetersPerPixel { get; set; }

    [JsonPropertyName("roi_polygon_json")]
    public string? RoiPolygonJson { get; set; }
}

public sealed class MlAnalyzeResponse
{
    [JsonPropertyName("frames_processed")]
    public int FramesProcessed { get; set; }

    [JsonPropertyName("processing_time_ms")]
    public int ProcessingTimeMs { get; set; }

    [JsonPropertyName("violations")]
    public List<MlViolationItem> Violations { get; set; } = [];
}

public sealed class MlViolationItem
{
    [JsonPropertyName("violation_type")]
    public string ViolationType { get; set; } = string.Empty;

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }

    [JsonPropertyName("frame_index")]
    public int FrameIndex { get; set; }

    [JsonPropertyName("frame_time_seconds")]
    public double FrameTimeSeconds { get; set; }

    [JsonPropertyName("track_id")]
    public int? TrackId { get; set; }

    [JsonPropertyName("object_class")]
    public string? ObjectClass { get; set; }

    [JsonPropertyName("bbox")]
    public MlBbox? Bbox { get; set; }

    [JsonPropertyName("frame_image_path")]
    public string? FrameImagePath { get; set; }
}

public sealed class MlBbox
{
    [JsonPropertyName("x1")]
    public double X1 { get; set; }

    [JsonPropertyName("y1")]
    public double Y1 { get; set; }

    [JsonPropertyName("x2")]
    public double X2 { get; set; }

    [JsonPropertyName("y2")]
    public double Y2 { get; set; }
}
