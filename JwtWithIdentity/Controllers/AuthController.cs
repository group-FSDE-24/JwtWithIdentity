using System.Net;
using Microsoft.AspNetCore.Mvc;
using JwtWithIdentity.Models.DTOS;
using Microsoft.AspNetCore.Identity;
using JwtWithIdentity.Services.Abstracts;

namespace JwtWithIdentity.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager)
    {
        _authService = authService;
        _roleManager = roleManager;
    }


    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(requestDTO);

        if (result.HttpStatusCode == HttpStatusCode.NotFound)
            return NotFound("User tapilmadi");

        else if (result.HttpStatusCode == HttpStatusCode.Unauthorized)
            return Unauthorized("Sifre yanlisdir");
        
        return Ok(result);

    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO requestDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(requestDTO);

        if (!result)
            return BadRequest("Qeydiyyat tamamlanmadi...");

        return Ok("Qeydiyyat tamamlandi");
    }

    [HttpPost("CreateRoles")]
    public async Task<IActionResult> CreateRoles()
    {
        await _roleManager.CreateAsync(new IdentityRole("Admin"));
        await _roleManager.CreateAsync(new IdentityRole("Student"));
        await _roleManager.CreateAsync(new IdentityRole("SuperStarAdmin"));

        return Ok("Role-lar yaradildi");
    }




}
