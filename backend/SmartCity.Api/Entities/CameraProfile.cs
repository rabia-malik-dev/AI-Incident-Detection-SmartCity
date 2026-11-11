namespace SmartCity.Api.Entities;

public class CameraProfile
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? AllowedDirectionDeg { get; set; }
    public double? SpeedLimitKmh { get; set; }
    public double? MetersPerPixel { get; set; }
    public string? RoiPolygonJson { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Video> Videos { get; set; } = [];
}
