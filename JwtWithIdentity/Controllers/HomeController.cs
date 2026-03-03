using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace JwtWithIdentity.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HomeController : ControllerBase
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public IActionResult TestAuthorize()
    {
        return Ok("Her 1 sey ugurludur...");
    }



    // [HttpGet("Esger")]
    // [AllowAnonymous]
    // public IActionResult Esger()
    // {
    //     _logger.LogInformation("Esger methodu fealiyyete baslayir...");
    // 
    //     try
    //     {
    //         throw new Exception("Aydin senede qismet");
    //     }
    //     catch (Exception ex)
    //     {
    //         _logger.LogError($"Exception occured: {ex.Message}");
    //         return BadRequest(ex.Message);
    //     }
    // 
    //     _logger.LogInformation("Esger methodu fealiyyete bitirdi...");
    //     return Ok("548 gun, yada 365 gun. Konul isterdiki Nigodnu olasan!!!");
    // }

    [HttpGet("Kesilenler")]
    [AllowAnonymous]
    public IActionResult Kesilenler()
    {
        Log.Information("Kesilenler methodu ise baslayir...");

        Log.Information("Kesilenler methodu ise bitir...");
        
        return Ok("Aydin, Muhammed, AliHuseyn, Nigar");


    }
}
