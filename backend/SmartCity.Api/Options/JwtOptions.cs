namespace SmartCity.Api.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = "SmartCityApi";
    public string Key { get; set; } = string.Empty;
    public int ExpiryMinutes { get; set; } = 480;
}
