using Asp.Versioning;
using FonTech.Domain.Dto.Role;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

/// <summary>
/// Контроллер для модели Role
/// </summary>
[Authorize(Roles = "Administrator, Moderator")]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
	private readonly IRoleService _roleService;

    /// <summary>
    /// Конструктор для инициализации зависимостей
    /// </summary>
    /// <param name="roleService"></param>
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

	/// <summary>
	/// Получить все роли
	/// </summary>
	/// <returns></returns>
    [HttpGet("roles")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<RoleDto>>> GetRoles()
	{
        var response = await _roleService.GetRolesAsync();

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
	}

	/// <summary>
	/// Получить все роли пользователя
	/// </summary>
	/// <param name="userId">Идентификатор пользователя</param>
	/// <remarks>
	/// Sample request:
	/// 
	///		GET
	///		{
	///			"userId": 1
	///		}
	/// </remarks>
	[HttpGet("roles/user/{userId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<RoleDto>>> GetUserRoles(int userId)
    {
		var response = await _roleService.GetUserRolesAsync(userId);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Получить роль по идентификатору
	/// </summary>
	/// <param name="id">Идентификатор роли</param>
	/// <remarks>
	/// Sample request:
	/// 
	///		GET
	///		{
	///			"id": 1
	///		}
	/// </remarks>
	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<RoleDto>>> GetRole(int id)
    {
		var response = await _roleService.GetRoleByIdAsync(id);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Создать роль
	/// </summary>
	/// <param name="dto"></param>
	/// <remarks>
	/// Sample request:
	/// 
	///		POST
	///		{
	///			"name": "Sample role"
	///		}
	/// </remarks>
	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<RoleDto>>> CreateRole([FromBody] RoleDto dto)
	{
		var response = await _roleService.CreateRoleAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Добавить роль пользователю
	/// </summary>
	/// <param name="dto"></param>
	/// <remarks>
	/// Sample request:
	/// 
	///		POST
	///		{
	///			"login": "UserLogin",
	///			"role": "SampleRole"
	///		}
	/// </remarks>
	[HttpPost("user")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<UserRoleDto>>> AddRoleForUser([FromBody] UserRoleDto dto)
	{
		var response = await _roleService.AddRoleForUserAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Обновить роль
	/// </summary>
	/// <param name="dto"></param>
	/// <remarks>
	/// Sample request:
	/// 
	///		PUT
	///		{
	///			"id": 1,
	///			"name": "NewRoleName"
	///		}
	/// </remarks>
	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<RoleDto>>> UpdateRole([FromBody] UpdateRoleDto dto)
	{
		var response = await _roleService.UpdateRoleAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	/// <summary>
	/// Удалить роль
	/// </summary>
	/// <param name="id">Идентификатор роли</param>
	/// <remarks>
	/// Sample request:
	/// 
	///		DELETE
	///		{
	///			"id": 1
	///		}
	/// </remarks>
	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<RoleDto>>> DeleteRole(int id)
	{
		var response = await _roleService.DeleteRoleAsync(id);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}
}
