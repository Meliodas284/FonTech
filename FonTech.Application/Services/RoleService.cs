using FonTech.Application.Resources;
using FonTech.Domain.Dto.Role;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;

namespace FonTech.Application.Services;

public class RoleService : IRoleService
{
	private readonly IBaseRepository<Role> _roleRepository;
	private readonly IBaseRepository<User> _userRepository;

	public RoleService(
		IBaseRepository<Role> roleRepository, 
		IBaseRepository<User> userRepository)
    {
		_roleRepository = roleRepository;
		_userRepository = userRepository;
    }

	/// <inheritdoc />
	public async Task<BaseResult<Role>> CreateRoleAsync(RoleDto dto)
	{
		var role = await _roleRepository.GetAll()
			.FirstOrDefaultAsync(x => x.Name == dto.Name);

		if (role != null)
		{
			return new BaseResult<Role>
			{
				ErrorCode = (int)ErrorCodes.RoleAlreadyExists,
				ErrorMessage = ErrorMessage.RoleAlreadyExists
			};
		}

		role = new Role
		{
			Name = dto.Name
		};

		await _roleRepository.CreateAsync(role);
		
		return new BaseResult<Role>
		{ 
			Data = role
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<Role>> DeleteRoleAsync(long id)
	{
		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

		if (role == null)
		{
			return new BaseResult<Role>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		await _roleRepository.DeleteAsync(role);

		return new BaseResult<Role> 
		{ 
			Data = role 
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<Role>> GetRoleByIdAsync(long id)
	{
		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

		if (role == null)
		{
			return new BaseResult<Role>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		return new BaseResult<Role>
		{
			Data = role
		};
	}

	/// <inheritdoc />
	public async Task<CollectionResult<Role>> GetRolesAsync()
	{
		var roles = await _roleRepository.GetAll().ToListAsync();

		if (!roles.Any())
		{
			return new CollectionResult<Role>
			{
				ErrorCode = (int)ErrorCodes.RolesNotFound,
				ErrorMessage = ErrorMessage.RolesNotFound
			};
		}

		return new CollectionResult<Role>() 
		{ 
			Data = roles
		};
	}

	/// <inheritdoc />
	public async Task<CollectionResult<Role>> GetUserRolesAsync(long userId)
	{
		var user = await _userRepository
			.GetAll()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Id == userId);

		if (user == null)
		{
			return new CollectionResult<Role>
			{
				ErrorCode = (int)ErrorCodes.UserNotFound,
				ErrorMessage = ErrorMessage.UserNotFound
			};
		}

		if (!user.Roles.Any())
		{
			return new CollectionResult<Role>
			{
				ErrorCode = (int)ErrorCodes.RolesNotFound,
				ErrorMessage = ErrorMessage.RolesNotFound
			};
		}

		return new CollectionResult<Role>
		{
			Data = user.Roles
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<Role>> UpdateRoleAsync(UpdateRoleDto dto)
	{
		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);

		if (role == null)
		{
			return new BaseResult<Role>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		role.Name = dto.Name;
		await _roleRepository.UpdateAsync(role);

		return new BaseResult<Role>
		{
			Data = role
		};
	}
}
