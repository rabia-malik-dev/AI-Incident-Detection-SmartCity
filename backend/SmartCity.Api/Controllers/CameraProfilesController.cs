using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Contracts;
using SmartCity.Api.Data;

namespace SmartCity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/camera-profiles")]
public class CameraProfilesController(IncidentDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CameraProfileDto>>> List(CancellationToken ct)
    {
        var items = await db.CameraProfiles
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CameraProfileDto(c.Id, c.Name, c.AllowedDirectionDeg, c.SpeedLimitKmh, c.MetersPerPixel))
            .ToListAsync(ct);
        return Ok(items);
    }
}
