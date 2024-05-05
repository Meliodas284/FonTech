using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FonTech.Application.Services;

public class TokenService : ITokenService
{
	private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
		_jwtSettings = jwtSettings.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var securityToken = new JwtSecurityToken(
			issuer: _jwtSettings.Issuer,
			audience: _jwtSettings.Audience,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(10),
			signingCredentials: credentials);
		var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

		return token;
	}

	public string GenerateRefreshToken()
	{
		var randomNumbers = new byte[32];
		using var randomNumberGenerator = RandomNumberGenerator.Create();
		randomNumberGenerator.GetBytes(randomNumbers);

		return Convert.ToBase64String(randomNumbers);
	}
}
