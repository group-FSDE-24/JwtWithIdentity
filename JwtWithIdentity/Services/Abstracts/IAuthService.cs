using JwtWithIdentity.Models.DTOS;

namespace JwtWithIdentity.Services.Abstracts;

public interface IAuthService
{
    // Login
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO model);
    // Register
    Task<bool> RegisterAsync(RegisterRequestDTO model);
}
