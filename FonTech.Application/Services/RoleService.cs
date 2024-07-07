using AutoMapper;
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
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public RoleService(
		IUnitOfWork unitOfWork,
		IMapper mapper)
    {
		_unitOfWork = unitOfWork;
		_mapper = mapper;
    }

	/// <inheritdoc />
	public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto dto)
	{
		var user = await _unitOfWork.Users.GetAll()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Login == dto.Login);

		if (user == null)
		{
			return new BaseResult<UserRoleDto>
			{
				ErrorCode = (int)ErrorCodes.UserNotFound,
				ErrorMessage = ErrorMessage.UserNotFound
			};
		}

		var role = await _unitOfWork.Roles.GetAll()
			.FirstOrDefaultAsync(x => x.Name == dto.Role);

		if (role == null)
		{
			return new BaseResult<UserRoleDto>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		var userRoles = user.Roles.Select(x => x.Name).ToArray();
		if (userRoles.Contains(dto.Role))
		{
			return new BaseResult<UserRoleDto>
			{
				ErrorCode = (int)ErrorCodes.UserAlreadyHasThisRole,
				ErrorMessage = ErrorMessage.UserAlreadyHasThisRole
			};
		}

		user.Roles.Add(role);
		await _unitOfWork.Users.UpdateAsync(user);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<UserRoleDto>
		{
			Data = dto
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<RoleDto>> CreateRoleAsync(RoleDto dto)
	{
		var role = await _unitOfWork.Roles.GetAll()
			.FirstOrDefaultAsync(x => x.Name == dto.Name);

		if (role != null)
		{
			return new BaseResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.RoleAlreadyExists,
				ErrorMessage = ErrorMessage.RoleAlreadyExists
			};
		}

		role = new Role
		{
			Name = dto.Name
		};

		await _unitOfWork.Roles.CreateAsync(role);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<RoleDto>
		{
			Data = dto
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
	{
		var role = await _unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == id);

		if (role == null)
		{
			return new BaseResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		await _unitOfWork.Roles.DeleteAsync(role);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<RoleDto> 
		{ 
			Data = _mapper.Map<RoleDto>(role)
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<RoleDto>> GetRoleByIdAsync(long id)
	{
		var role = await _unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == id);

		if (role == null)
		{
			return new BaseResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		return new BaseResult<RoleDto>
		{
			Data = _mapper.Map<RoleDto>(role)
		};
	}

	/// <inheritdoc />
	public async Task<CollectionResult<RoleDto>> GetRolesAsync()
	{
		var roles = await _unitOfWork.Roles.GetAll().ToListAsync();

		if (!roles.Any())
		{
			return new CollectionResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.RolesNotFound,
				ErrorMessage = ErrorMessage.RolesNotFound
			};
		}

		return new CollectionResult<RoleDto>
		{ 
			Data = roles.Select(x => _mapper.Map<RoleDto>(x)),
			Count = roles.Count
		};
	}

	/// <inheritdoc />
	public async Task<CollectionResult<RoleDto>> GetUserRolesAsync(long userId)
	{
		var user = await _unitOfWork.Users
			.GetAll()
			.Include(x => x.Roles)
			.FirstOrDefaultAsync(x => x.Id == userId);

		if (user == null)
		{
			return new CollectionResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.UserNotFound,
				ErrorMessage = ErrorMessage.UserNotFound
			};
		}

		if (!user.Roles.Any())
		{
			return new CollectionResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.RolesNotFound,
				ErrorMessage = ErrorMessage.RolesNotFound
			};
		}

		return new CollectionResult<RoleDto>
		{
			Data = user.Roles.Select(x => _mapper.Map<RoleDto>(x)),
			Count = user.Roles.Count
		};
	}

	/// <inheritdoc />
	public async Task<BaseResult<RoleDto>> UpdateRoleAsync(UpdateRoleDto dto)
	{
		var role = await _unitOfWork.Roles.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);

		if (role == null)
		{
			return new BaseResult<RoleDto>
			{
				ErrorCode = (int)ErrorCodes.RoleNotFound,
				ErrorMessage = ErrorMessage.RoleNotFound
			};
		}

		role.Name = dto.Name;
		await _unitOfWork.Roles.UpdateAsync(role);
		await _unitOfWork.SaveChangeAsync();

		return new BaseResult<RoleDto>
		{
			Data = _mapper.Map<RoleDto>(role)
		};
	}
}
