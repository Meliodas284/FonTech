using FonTech.Application.Resources;
using FonTech.Domain.Dto.Role;
using FonTech.Domain.Entity;
using FonTech.Domain.Exceptions;
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

    public async Task<BaseResult<Role>> CreateRoleAsync(RoleDto dto)
	{
		var role = await _roleRepository.GetAll()
			.FirstOrDefaultAsync(x => x.Name == dto.Name);

		if (role != null)
		{
			throw new RoleAlreadyExistsException(ErrorMessage.RoleAlreadyExists);
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

	public async Task<BaseResult<Role>> DeleteRoleAsync(long id)
	{
		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

		if (role == null)
		{
			throw new RolesNotFoundException(ErrorMessage.RoleNotFound);
		}

		await _roleRepository.DeleteAsync(role);

		return new BaseResult<Role> 
		{ 
			Data = role 
		};
	}

	public async Task<BaseResult<Role>> GetRoleByIdAsync(long id)
	{
		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);

		if (role == null)
		{
			throw new RolesNotFoundException(ErrorMessage.RoleNotFound);
		}

		return new BaseResult<Role>
		{
			Data = role
		};
	}

	public async Task<CollectionResult<Role>> GetRolesAsync()
	{
		var roles = await _roleRepository.GetAll().ToListAsync();

		if (!roles.Any())
		{
			throw new RolesNotFoundException(ErrorMessage.RoleNotFound);
		}

		return new CollectionResult<Role>() 
		{ 
			Data = roles
		};
	}

	public async Task<CollectionResult<Role>> GetUserRolesAsync(long userId)
	{
		var user = await _userRepository
			.GetAll()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Id == userId);

		if (user == null)
		{
			throw new UserNotFoundException(ErrorMessage.UserNotFound);
		}

		if (!user.Roles.Any())
		{
			throw new RolesNotFoundException(ErrorMessage.RoleNotFound);
		}

		return new CollectionResult<Role>
		{
			Data = user.Roles
		};
	}

	public async Task<BaseResult<Role>> UpdateRoleAsync(UpdateRoleDto dto)
	{
		var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);

		if (role == null)
		{
			throw new RolesNotFoundException(ErrorMessage.RoleNotFound);
		}

		role.Name = dto.Name;
		await _roleRepository.UpdateAsync(role);

		return new BaseResult<Role>
		{
			Data = role
		};
	}
}
