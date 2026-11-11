using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SmartCity.Api.Contracts.Ml;
using SmartCity.Api.Options;

namespace SmartCity.Api.Services;

public class MlAnalysisClient(HttpClient http, IOptions<MlServiceOptions> options, ILogger<MlAnalysisClient> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly MlServiceOptions _options = options.Value;

    public async Task<MlAnalyzeResponse> AnalyzeVideoAsync(
        string videoPath,
        string originalFileName,
        MlCameraProfilePayload cameraProfile,
        CancellationToken ct)
    {
        await using var stream = File.OpenRead(videoPath);
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
        content.Add(fileContent, "file", originalFileName);

        var profileJson = JsonSerializer.Serialize(cameraProfile);
        content.Add(new StringContent(profileJson), "camera_profile_json");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/analyze/video") { Content = content };
        using var response = await http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("ML service error {Status}: {Body}", response.StatusCode, body);
            throw new InvalidOperationException($"ML service returned {(int)response.StatusCode}: {body}");
        }

        var parsed = JsonSerializer.Deserialize<MlAnalyzeResponse>(body, JsonOptions)
                     ?? throw new InvalidOperationException("Empty ML response.");
        return parsed;
    }
}
