using FonTech.Domain.Dto;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Result;

namespace FonTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис, предназначенный для авторизации и регистрации
/// </summary>
public interface IAuthService
{
	/// <summary>
	/// Регистрация пользователя
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<UserDto>> RegisterAsync(RegisterUserDto dto);

	/// <summary>
	/// Авторизация пользователя
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<TokenDto>> LoginAsync(LoginUserDto dto);
}
