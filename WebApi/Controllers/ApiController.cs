using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api")]
[Authorize]
public class ApiController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("anonymous")]
    public IActionResult Anonymous()
    {
       return Ok(new { Message = "You don't need authentication !" });
    }

    [Authorize(Roles = "admin")]
    [HttpGet("manage")]
    public IActionResult AdminOnly()
    {
        return Ok(new { Message = "You are authorized as admin !" });
    }

    [Authorize(Roles = "admin, reader")]
    [HttpGet("read")]
    public IActionResult AdminOrReader()
    {
        return Ok(new { Message = "You are authorized as admin or reader !" });
    }
}
