using System.Text;
using JwtWithIdentity.Models.DTOS;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using JwtWithIdentity.Services.Abstracts;

namespace JwtWithIdentity.Services.Concretes;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(TokenRequestDTO requestDTO)
    {
        var jwtAccessToken = new JwtSecurityToken(
            issuer: _configuration["JWT:IsSuer"],
            audience: _configuration["JWT:Audience"],
            claims: requestDTO.Claims,
            expires: DateTime.Now.AddMinutes(2),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"])
                    ), SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
    }

    public string GenerateRefreshToken()
     => Guid.NewGuid().ToString().ToLower();
}
