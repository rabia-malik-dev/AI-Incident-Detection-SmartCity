namespace SmartCity.Api.Options;

public class StorageOptions
{
    public const string SectionName = "Storage";

    public string UploadsPath { get; set; } = "./data/uploads";
    public string SnapshotsPath { get; set; } = "./data/snapshots";
    public long MaxUploadBytes { get; set; } = 200 * 1024 * 1024;
}
