using FonTech.Domain.Dto.Role;
using FonTech.Domain.Entity;
using FonTech.Domain.Result;

namespace FonTech.Domain.Interfaces.Services;

/// <summary>
/// Сервис управляющий ролями
/// </summary>
public interface IRoleService
{
	/// <summary>
	/// Получить все роли
	/// </summary>
	/// <returns></returns>
	Task<CollectionResult<RoleDto>> GetRolesAsync();

	/// <summary>
	/// Получить роль по Id
	/// </summary>
	/// <param name="id">Id роли</param>
	/// <returns></returns>
	Task<BaseResult<RoleDto>> GetRoleByIdAsync(long id);
	
	/// <summary>
	/// Создать роль
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<RoleDto>> CreateRoleAsync(RoleDto dto);

	/// <summary>
	/// Удалить роль по Id
	/// </summary>
	/// <param name="id">Id роли</param>
	/// <returns></returns>
	Task<BaseResult<RoleDto>> DeleteRoleAsync(long id);

	/// <summary>
	/// Обновить роль
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<RoleDto>> UpdateRoleAsync(UpdateRoleDto dto);

	/// <summary>
	/// Получить все роли пользователя
	/// </summary>
	/// <param name="userId">Id пользователя</param>
	/// <returns></returns>
	Task<CollectionResult<RoleDto>> GetUserRolesAsync(long userId);

	/// <summary>
	/// Добавить роль пользователю
	/// </summary>
	/// <param name="dto"></param>
	/// <returns></returns>
	Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto);
}
