using Application.Models;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Course -> CourseDTO
        CreateMap<Course, CourseDto>()
            // .ForMember(dest => dest.ModuleNames, opt => opt.MapFrom(src => src.Modules.Select(m => m.Name).ToArray()))
            .ConstructUsing(src => new CourseDto(
                src.Id,
                src.Name,
                src.Description,
                src.StartDate,
                src.EndDate
                // src.Modules.Select(m => m.Name).ToArray()
                ))
            .ReverseMap();

        //Activity -> ActivityDTO
        CreateMap<Activity, ActivityDto>()
            .ForMember(dest => dest.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityType.Name))
            .ForMember(dest => dest.ActivityTypeDescription, opt => opt.MapFrom(src => src.ActivityType.Description))
            .ReverseMap();

        //Module -> ModuleDTO
        CreateMap<Module, ModuleDto>()
            .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
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
        CreateMap<Course, CourseCreateDto>().ReverseMap();

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
