using FonTech.Application.Resources;
using FonTech.Domain.Dto;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using FonTech.Domain.Settings;
using Microsoft.EntityFrameworkCore;
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
	private readonly IBaseRepository<User> _userRepository;

    public TokenService(IOptions<JwtSettings> jwtSettings, IBaseRepository<User> userRepository)
    {
		_jwtSettings = jwtSettings.Value;
		_userRepository = userRepository;
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

	public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
	{
		var tokenValidationParameters = new TokenValidationParameters()
		{
			ValidateAudience = true,
			ValidateIssuer = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.JwtKey)),
			ValidAudience = _jwtSettings.Audience,
			ValidIssuer = _jwtSettings.Issuer
		};
		
		var tokenHandler = new JwtSecurityTokenHandler();
		var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
		if (securityToken is not JwtSecurityToken jwtSecurityToken || 
			!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
		{
			throw new SecurityTokenException(ErrorMessage.InvalidToken);
		}

		return claimsPrincipal;
	}

	public async Task<BaseResult<TokenDto>> RefreshToken(TokenDto dto)
	{
		var claimsPrincipal = GetPrincipalFromExpiredToken(dto.AccessToken);
		var userName = claimsPrincipal.Identity!.Name;
		var user = await _userRepository.GetAll()
			.Include(x => x.UserToken)
			.FirstOrDefaultAsync(x => x.Login == userName);

		if (user == null || user.UserToken.RefreshToken != dto.RefreshToken ||
			user.UserToken.RefreshTokenExpiryTime <= DateTime.UtcNow)
		{
			return new BaseResult<TokenDto>
			{
				ErrorCode = (int)ErrorCodes.InvalidClientRequest,
				ErrorMessage = ErrorMessage.InvalidClientRequest
			};
		}

		var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
		var newRefreshToken = GenerateRefreshToken();
		user.UserToken.RefreshToken = newRefreshToken;
		await _userRepository.UpdateAsync(user);

		return new BaseResult<TokenDto>()
		{
			Data = new TokenDto
			{
				AccessToken = newAccessToken,
				RefreshToken = newRefreshToken
			}
		};
	}
}
