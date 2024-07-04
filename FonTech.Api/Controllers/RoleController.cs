using Asp.Versioning;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

/// <summary>
/// Контроллер для модели Role
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
	private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
	public async Task<ActionResult<CollectionResult<Role>>> GetRoles()
	{
        var response = await _roleService.GetRolesAsync();

        return Ok(response);
	}
}
