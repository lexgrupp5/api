using AutoMapper;

using Domain.DTOs;
using Domain.Entities;

namespace Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Course, CourseDto>()
            .ConstructUsing(src => new CourseDto(
                src.Id,
                src.Name,
                src.Description,
                src.StartDate,
                src.EndDate
                //src.Modules.Select(m => m.Name).ToList()
            ))
            .ReverseMap();
        CreateMap<Module, ModuleDto>();
        CreateMap<Activity, ActivityDto>();
        CreateMap<User, UserDto>();
        CreateMap<Role, RoleDto>();
    }
}
