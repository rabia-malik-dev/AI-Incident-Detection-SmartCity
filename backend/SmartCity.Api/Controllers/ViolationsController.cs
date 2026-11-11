using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Contracts;
using SmartCity.Api.Data;
using SmartCity.Api.Extensions;

namespace SmartCity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/violations")]
public class ViolationsController(IncidentDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ViolationDto>>> List(
        [FromQuery] string? violationType,
        [FromQuery] int? videoId,
        [FromQuery] int limit = 100,
        CancellationToken ct = default)
    {
        limit = Math.Clamp(limit, 1, 500);
        var userId = User.GetUserId();

        var query = db.Violations
            .AsNoTracking()
            .Where(v => v.Video.UserId == userId);

        if (!string.IsNullOrWhiteSpace(violationType))
            query = query.Where(v => v.ViolationType == violationType);
        if (videoId.HasValue)
            query = query.Where(v => v.VideoId == videoId.Value);

        var list = await query
            .OrderByDescending(v => v.DetectedAtUtc)
            .Take(limit)
            .Select(v => new ViolationDto(
                v.Id,
                v.VideoId,
                v.ViolationType,
                v.Confidence,
                v.DetectedAtUtc,
                v.FrameIndex,
                v.FrameTimeSeconds,
                v.TrackId,
                v.ObjectClass,
                SnapshotUrlHelper.ToPublicUrl(v.FrameImagePath)))
            .ToListAsync(ct);

        return Ok(list);
    }
}

internal static class SnapshotUrlHelper
{
    public static string? ToPublicUrl(string? frameImagePath)
    {
        if (string.IsNullOrWhiteSpace(frameImagePath))
            return null;
        var name = Path.GetFileName(frameImagePath);
        return string.IsNullOrEmpty(name) ? null : $"/api/snapshots/{name}";
    }
}
