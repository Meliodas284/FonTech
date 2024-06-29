using AutoMapper;
using FonTech.Application.Resources;
using FonTech.Domain.Dto;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Exceptions;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FonTech.Application.Services;

public class AuthService : IAuthService
{
	private readonly IBaseRepository<User> _userRepository;
	private readonly IBaseRepository<UserToken> _userTokenRepository;
	private readonly ITokenService _tokenService;
	private readonly ILogger _logger;
	private readonly IMapper _mapper;

    public AuthService(
		IBaseRepository<User> userRepository, 
		IBaseRepository<UserToken> userTokenRepository,
		ITokenService tokenService,
		ILogger logger, 
		IMapper mapper)
    {
		_userRepository = userRepository;
		_userTokenRepository = userTokenRepository;
		_tokenService = tokenService;
		_logger = logger;
		_mapper = mapper;
    }

    /// <inheritdoc />
	public async Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto)
	{
		var user = await _userRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Login == dto.Login);

		if (user == null)
		{
			throw new UserNotFoundException(ErrorMessage.UserNotFound);
		}

		if (!IsVerifiedPassword(dto.Password, user.Password))
		{
			throw new WrongPasswordException(ErrorMessage.WrongPassword);
		}

		var userToken = await _userTokenRepository.GetAll()
			.FirstOrDefaultAsync(x => x.UserId == user.Id);

		var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.Login),
				new Claim(ClaimTypes.Role, "User")
			};
		var accessToken = _tokenService.GenerateAccessToken(claims);
		var refreshToken = _tokenService.GenerateRefreshToken();

		if (userToken == null)
		{
			userToken = new UserToken()
			{
				UserId = user.Id,
				RefreshToken = refreshToken,
				RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
			};

			await _userTokenRepository.CreateAsync(userToken);
		}
		else
		{
			userToken.RefreshToken = refreshToken;
			userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

			await _userTokenRepository.UpdateAsync(userToken);
		}

		return new BaseResult<TokenDto>()
		{
			Data = new TokenDto()
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			}
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<UserDto>> RegisterAsync(RegisterUserDto dto)
	{
		if (dto.Password != dto.PasswordConfirm)
		{
			throw new PasswordNotEqualsException(ErrorMessage.PasswordNotEquals);
		}

		var user = await _userRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Login == dto.Login);

		if (user != null)
		{
			throw new UserAlreadyExistsException(ErrorMessage.UserAlreadyExists);
		}

		var hashPassword = HashPassword(dto.Password);

		user = new User
		{
			Login = dto.Login,
			Password = hashPassword
		};

		await _userRepository.CreateAsync(user);

		return new BaseResult<UserDto>()
		{
			Data = _mapper.Map<UserDto>(user)
		};
	}

	private string HashPassword(string password)
	{
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

		return Convert.ToBase64String(bytes);
	}

	private bool IsVerifiedPassword(string userPassword, string userPasswordHash)
	{
		var hash = HashPassword(userPassword);

		return hash == userPasswordHash;
	}
}
