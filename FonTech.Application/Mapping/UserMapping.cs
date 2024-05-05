using AutoMapper;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Entity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FonTech.Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>()
			.ForCtorParam(ctorParamName: "Login", m => m.MapFrom(s => s.Login))
			.ReverseMap();
    }
}
