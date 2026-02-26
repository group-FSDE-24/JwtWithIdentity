using System.Net;
using System.Security.Claims;
using JwtWithIdentity.Models.DTOS;
using Microsoft.AspNetCore.Identity;
using JwtWithIdentity.Services.Abstracts;
using JwtWithIdentity.Models.Entities.Concretes;

namespace JwtWithIdentity.Services.Concretes;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AuthService(ITokenService tokenService, UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            return new LoginResponseDTO() { HttpStatusCode = HttpStatusCode.NotFound };

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (!result.Succeeded)
            return new LoginResponseDTO() { HttpStatusCode = HttpStatusCode.Unauthorized };

        var tokenRequestDTO = new TokenRequestDTO()
        {
            Roles = await _userManager.GetRolesAsync(user),
            Claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, string.Join( ',', await _userManager.GetRolesAsync(user)))
            }
        };

        var accessToken = _tokenService.GenerateAccessToken(tokenRequestDTO);

        return new LoginResponseDTO()
        {
            AccessToken = accessToken,
            HttpStatusCode = HttpStatusCode.OK
        };
    }

    public async Task<bool> RegisterAsync(RegisterRequestDTO model)
    {
        var existUser = await _userManager.FindByEmailAsync(model.Email);

        if (existUser is not null)
            return false;

        var newUser = new User()
        {
            Email = model.Email,
            UserName = model.Username,
            Name = model.FirstName,
            Surname = model.LastName
        };


        var result = await _userManager.CreateAsync(newUser, model.Password);

        await _userManager.AddToRoleAsync(newUser, model.RoleName);

        return result.Succeeded;
    }
}
