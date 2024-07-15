using AutoMapper;
using FluentValidation;
using FonTech.Application.Extensions;
using FonTech.Application.Resources;
using FonTech.Domain.Dto;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FonTech.Application.Services;

public class AuthService : IAuthService
{
	private readonly IBaseRepository<User> _userRepository;
	private readonly IBaseRepository<UserToken> _userTokenRepository;
	private readonly IBaseRepository<Role> _roleRepository;
	private readonly ITokenService _tokenService;
	private readonly IValidator<RegisterUserDto> _registerUserValidator;
	private readonly IValidator<LoginUserDto> _loginUserValidator;
	private readonly IMapper _mapper;

    public AuthService(
		IBaseRepository<User> userRepository, 
		IBaseRepository<UserToken> userTokenRepository,
		IBaseRepository<Role> roleRepository,
		ITokenService tokenService,
		IValidator<RegisterUserDto> registerUserValidator,
		IValidator<LoginUserDto> loginUserValidator,
		IMapper mapper)
    {
		_userRepository = userRepository;
		_userTokenRepository = userTokenRepository;
		_roleRepository = roleRepository;
		_tokenService = tokenService;
		_registerUserValidator = registerUserValidator;
		_loginUserValidator = loginUserValidator;
		_mapper = mapper;
    }

    /// <inheritdoc />
	public async Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto)
	{
		var result = _loginUserValidator.Validate(dto);
		if (!result.IsValid)
		{
			return new BaseResult<TokenDto>
			{
				ErrorCode = (int)ErrorCodes.LoginDataIsNotValid,
				ErrorMessage = result.ToFormattedString()
			};
		}

		var user = await _userRepository.GetAll()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Login == dto.Login);

		if (user == null)
		{
			return new BaseResult<TokenDto>
			{
				ErrorCode = (int)ErrorCodes.UserNotFound,
				ErrorMessage = ErrorMessage.UserNotFound
			};
		}

		if (!IsVerifiedPassword(dto.Password, user.Password))
		{
			return new BaseResult<TokenDto>
			{
				ErrorCode = (int)ErrorCodes.WrongPassword,
				ErrorMessage = ErrorMessage.WrongPassword
			};
		}

		var userToken = await _userTokenRepository.GetAll()
			.FirstOrDefaultAsync(x => x.UserId == user.Id);

		var claims = user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Name)).ToList();
		claims.Add(new Claim(ClaimTypes.Name, user.Login));

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
		var result = _registerUserValidator.Validate(dto);
		if (!result.IsValid)
		{
			return new BaseResult<UserDto>
			{
				ErrorCode = (int)ErrorCodes.RegisterDataIsNotValid,
				ErrorMessage = result.ToFormattedString()
			};
		}

		var user = await _userRepository.GetAll()
				.FirstOrDefaultAsync(x => x.Login == dto.Login);

		if (user != null)
		{
			return new BaseResult<UserDto>
			{
				ErrorCode = (int)ErrorCodes.UserAlreadyExists,
				ErrorMessage = ErrorMessage.UserAlreadyExists
			};
		}

		var hashPassword = HashPassword(dto.Password);

		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == "User");
		if (role == null)
		{
			return new BaseResult<UserDto>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		user = new User
		{
			Login = dto.Login,
			Password = hashPassword,
			Roles = new List<Role> { role }
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
