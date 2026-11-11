namespace SmartCity.Api.Options;

public class MlServiceOptions
{
    public const string SectionName = "MlService";

    public string BaseUrl { get; set; } = "http://localhost:8000";
    public int TimeoutMinutes { get; set; } = 15;
}
