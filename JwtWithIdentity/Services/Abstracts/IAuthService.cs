using JwtWithIdentity.Models.DTOS;

namespace JwtWithIdentity.Services.Abstracts;

public interface IAuthService
{
    // Login
    Task<LoginResponseDTO> Login(LoginRequestDTO model);
    // Register
    Task<bool> Register(RegisterRequestDTO model);
}
