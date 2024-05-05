using FonTech.Domain.Dto;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers
{
	/// <summary>
	/// Контроллер для аутентификации и авторизации
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

        /// <summary>
		/// Конструктор для инициализации зависимостей
		/// </summary>
		/// <param name="authService"></param>
		public AuthController(IAuthService authService)
        {
			_authService = authService;
        }

		/// <summary>
		/// Регистрация пользователя
		/// </summary>
		/// <param name="dto"></param>
		/// <remarks>
		///	Sample request:
		///	
		///		POST
		///		{
		///			"login": "UserLogin",
		///			"password": "UserPassword1234",
		///			"passwordConfirm": "UserPassword1234"
		///		}
		/// </remarks>
		[HttpPost("register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody]RegisterUserDto dto)
		{
			var response = await _authService.RegisterAsync(dto);

			if (response.IsSuccess)
				return Ok(response);

			return BadRequest(response);
		}

		/// <summary>
		/// Логирование пользователя
		/// </summary>
		/// <param name="dto"></param>
		/// <remarks>
		///	Sample request:
		///	
		///		POST
		///		{
		///			"login": "UserLogin",
		///			"password": "UserPassword1234"
		///		}
		/// </remarks>
		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody]LoginUserDto dto)
		{
			var response = await _authService.LoginAsync(dto);

			if (response.IsSuccess)
				return Ok(response);

			return BadRequest(response);
		}
	}
}
