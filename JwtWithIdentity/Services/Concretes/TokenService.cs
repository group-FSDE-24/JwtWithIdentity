using System.Text;
using JwtWithIdentity.Models.DTOS;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtWithIdentity.Configurations;
using System.IdentityModel.Tokens.Jwt;
using JwtWithIdentity.Services.Abstracts;

namespace JwtWithIdentity.Services.Concretes;

public class TokenService : ITokenService
{
    private readonly JWTConfig _jWTConfig;

    public TokenService(IOptions<JWTConfig> jwtConfig)
    {
        _jWTConfig = jwtConfig.Value;
    }

    public string GenerateAccessToken(TokenRequestDTO requestDTO)
    {
        var jwtAccessToken = new JwtSecurityToken(
            issuer: _jWTConfig.IsSuer,
            audience: _jWTConfig.Audience,
            claims: requestDTO.Claims,
            expires: DateTime.Now.AddMinutes(2),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jWTConfig.SecretKey)
                    ), SecurityAlgorithms.HmacSha256)
            );

        return new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
    }

    public string GenerateRefreshToken()
     => Guid.NewGuid().ToString().ToLower();
}
