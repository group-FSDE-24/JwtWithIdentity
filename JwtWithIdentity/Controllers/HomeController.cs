using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace JwtWithIdentity.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult TestAuthorize()
    {
        return Ok("Her 1 sey ugurludur...");
    }
}
