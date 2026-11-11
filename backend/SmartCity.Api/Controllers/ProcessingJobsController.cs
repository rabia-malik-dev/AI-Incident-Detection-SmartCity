using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Contracts;
using SmartCity.Api.Data;
using SmartCity.Api.Extensions;

namespace SmartCity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/processing-jobs")]
public class ProcessingJobsController(IncidentDbContext db) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProcessingJobDto>> Get(int id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var job = await db.ProcessingJobs
            .AsNoTracking()
            .Where(j => j.Id == id && j.Video.UserId == userId)
            .Select(j => new ProcessingJobDto(
                j.Id,
                j.VideoId,
                j.Status,
                j.StartedAtUtc,
                j.CompletedAtUtc,
                j.ErrorMessage,
                j.Violations.Count))
            .FirstOrDefaultAsync(ct);

        return job is null ? NotFound() : Ok(job);
    }
}
