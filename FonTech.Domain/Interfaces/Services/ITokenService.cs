using System.Security.Claims;

namespace FonTech.Domain.Interfaces.Services;

public interface ITokenService
{
	string GenerateAccessToken(IEnumerable<Claim> claims);

	string GenerateRefreshToken();
}
