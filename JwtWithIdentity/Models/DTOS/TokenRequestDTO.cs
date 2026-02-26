using System.Security.Claims;

namespace JwtWithIdentity.Models.DTOS;

public class TokenRequestDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }

    public List<Claim> Claims { get; set; }
    public List<string> Roles { get; set; }
}
