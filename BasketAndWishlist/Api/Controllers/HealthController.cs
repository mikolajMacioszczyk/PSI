using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public ActionResult Health()
    {
        return Ok("healthy");
    }
}
