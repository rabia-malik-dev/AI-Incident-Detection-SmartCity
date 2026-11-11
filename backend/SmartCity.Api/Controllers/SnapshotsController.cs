using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmartCity.Api.Options;

namespace SmartCity.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/snapshots")]
public class SnapshotsController(IOptions<StorageOptions> storage) : ControllerBase
{
    [HttpGet("{fileName}")]
    public IActionResult Get(string fileName)
    {
        if (fileName.Contains('/') || fileName.Contains('\\') || fileName.Contains(".."))
            return BadRequest();

        var path = Path.Combine(storage.Value.SnapshotsPath, fileName);
        if (!System.IO.File.Exists(path))
            return NotFound();

        return PhysicalFile(path, "image/jpeg", enableRangeProcessing: false);
    }
}
