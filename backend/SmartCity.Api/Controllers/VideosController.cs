using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Contracts;
using SmartCity.Api.Data;
using SmartCity.Api.Extensions;
using SmartCity.Api.Services;

namespace SmartCity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/videos")]
public class VideosController(VideoIngestService ingest, IncidentDbContext db) : ControllerBase
{
    [HttpPost("upload")]
    [RequestSizeLimit(209_715_200)]
    public async Task<ActionResult<VideoUploadResponse>> Upload(
        IFormFile file,
        [FromForm] string? title,
        [FromForm] int? cameraProfileId,
        CancellationToken ct)
    {
        try
        {
            var userId = User.GetUserId();
            return Ok(await ingest.UploadAsync(userId, file, title, cameraProfileId, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<VideoListItem>>> List(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var items = await db.Videos
            .AsNoTracking()
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.UploadedAtUtc)
            .Select(v => new VideoListItem(
                v.Id,
                v.FileName,
                v.Title,
                v.UploadedAtUtc,
                v.CameraProfileId,
                v.ProcessingJobs
                    .OrderByDescending(j => j.CreatedAtUtc)
                    .Select(j => j.Status)
                    .FirstOrDefault() ?? "Pending"))
            .ToListAsync(ct);

        return Ok(items);
    }
}
