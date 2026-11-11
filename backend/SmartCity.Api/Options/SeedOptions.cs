namespace SmartCity.Api.Options;

public class SeedOptions
{
    public const string SectionName = "Seed";

    public bool Enabled { get; set; } = true;
    public string AdminEmail { get; set; } = "admin@smartcity.local";
    public string AdminPassword { get; set; } = "Admin@12345";
    public string AdminDisplayName { get; set; } = "System Admin";
}
