using FonTech.Domain.Dto;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

/// <summary>
/// Контроллер для модели Token
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
	private readonly ITokenService _tokenService;

    /// <summary>
    /// Конструктор для инициализации зависимостей
    /// </summary>
    /// <param name="tokenService"></param>
    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    /// <summary>
    /// Обновить токен
    /// </summary>
    /// <param name="dto"></param>
    [HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody]TokenDto dto)
    {
        var response = await _tokenService.RefreshToken(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }
}
