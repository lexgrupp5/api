using Application.Models;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {

        // Course
        CreateMap<Course, CourseDto>();
        CreateMap<Course, CourseCompleteDto>()
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.Modules))
            .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Users))
            .ReverseMap();
        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        //Activity -> ActivityDTO
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityType.Name))
            .ForMember(dest => dest.ActivityTypeDescription, opt => opt.MapFrom(src => src.ActivityType.Description))
            .ReverseMap();

        //Module -> ModuleDTO
        CreateMap<Module, ModuleDto>()
            .ConstructUsing(src => new ModuleDto(
                src.Id,
                src.CourseId,
                src.Name,
                src.Description,
                src.StartDate,
                src.EndDate,
                src.Activities.Select(a => new ActivityDto(
                    a.Id,
                    a.ModuleId,
                    a.Description,
                    a.StartDate,
                    a.EndDate,
                    a.ActivityType.Name,
                    a.ActivityType.Description
                )).ToList()
            ))
            .ReverseMap();

        CreateMap<ModuleCreateModel, Module>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Course, opt => opt.Ignore())
            .ForMember(dest => dest.Activities, opt => opt.Ignore())
            .ForMember(dest => dest.Documents, opt => opt.Ignore());
        CreateMap<Module, ModuleForCreationDto>().ReverseMap();
        CreateMap<ModuleCreateModel, ModuleForCreationDto>();

        CreateMap<UserCreateModel, UserForCreationDto>().ReverseMap();
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<ActivityCreateModel, Activity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Module, opt => opt.Ignore())
            .ForMember(dest => dest.ActivityType, opt => opt.Ignore());
        CreateMap<ActivityCreateModel, ActivityForCreationDto>().ReverseMap();
        CreateMap<ActivityForCreationDto, Activity>().ReverseMap();

        CreateMap<Module, ModuleToPatchDto>().ReverseMap();

        CreateMap<Module, ModuleDto>()
            .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities));
    }
}
