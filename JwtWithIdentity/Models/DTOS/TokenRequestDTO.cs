using System.Security.Claims;

namespace JwtWithIdentity.Models.DTOS;

public class TokenRequestDTO
{
    public List<Claim> Claims { get; set; }
    public IList<string> Roles { get; set; }
}
