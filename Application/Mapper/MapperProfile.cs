using Application.Models;

using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Course -> CourseDTO
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

        //Module -> ModuleDTO
        CreateMap<Module, ModuleDto>()
            //.ConstructUsing(src => new ModuleDto(
            //    src.Id,
            //    src.Name,
            //    src.Description,
            //    src.StartDate,
            //    src.EndDate
            //    ))
            .ReverseMap();

        CreateMap<UserCreateModel, UserForCreationDto>().ReverseMap();
        CreateMap<Activity, ActivityDto>().ReverseMap();
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserDto>() .ReverseMap();
    }
}
