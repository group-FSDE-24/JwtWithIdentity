using System.Net;

namespace JwtWithIdentity.Models.DTOS;

public class LoginResponseDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public HttpStatusCode HttpStatusCode { get; set; }
}
