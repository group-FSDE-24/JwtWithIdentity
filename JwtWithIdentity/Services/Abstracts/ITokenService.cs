using JwtWithIdentity.Models.DTOS;

namespace JwtWithIdentity.Services.Abstracts;

public interface ITokenService
{
    string GenerateAccessToken(TokenRequestDTO requestDTO);
    string GenerateRefreshToken();
}
