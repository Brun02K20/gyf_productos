using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
public sealed class StatusController : ControllerBase
{
    [HttpGet("/")]
    public IActionResult Root()
    {
        return Ok(new
        {
            status = "ok",
            service = "backend",
            utc = DateTime.UtcNow
        });
    }

    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok(new { status = "ok" });
    }
}