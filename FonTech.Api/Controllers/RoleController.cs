using Asp.Versioning;
using FonTech.Domain.Dto.Role;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

/// <summary>
/// Контроллер для модели Role
/// </summary>
[Authorize]
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

    [HttpGet("roles")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<Role>>> GetRoles()
	{
        var response = await _roleService.GetRolesAsync();

        if (response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
	}

    [HttpGet("roles/{userId}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<CollectionResult<Role>>> GetUserRoles(int userId)
    {
		var response = await _roleService.GetUserRolesAsync(userId);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> GetRole(int id)
    {
		var response = await _roleService.GetRoleByIdAsync(id);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> CreateRole([FromBody] RoleDto dto)
	{
		var response = await _roleService.CreateRoleAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> UpdateRole([FromBody] UpdateRoleDto dto)
	{
		var response = await _roleService.UpdateRoleAsync(dto);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<BaseResult<Role>>> DeleteRole(int id)
	{
		var response = await _roleService.DeleteRoleAsync(id);

		if (response.IsSuccess)
			return Ok(response);

		return BadRequest(response);
	}
}
