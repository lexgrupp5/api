using AutoMapper;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        /*
         * COURSE
         ***********/
        CreateMap<Course, Course>();
        CreateMap<Course, CourseDto>().ReverseMap();
        CreateMap<Course, CourseCompleteDto>()
            .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.Modules))
            .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Users))
            .ReverseMap();

        CreateMap<CourseCreateDto, Course>();
        CreateMap<CourseUpdateDto, Course>();

        /*
         * MODULE
         ***********/
        CreateMap<Module, Module>();
        CreateMap<Module, ModuleDto>()
            .ForMember(dest => dest.Activities, opt => opt.MapFrom(src => src.Activities))
            .ReverseMap();

        CreateMap<ModuleCreateDto, Module>();
        CreateMap<ModuleUpdateDto, Module>();

        /*
         * ACTIVITY
         ***********/
        CreateMap<Activity, Activity>();
        CreateMap<Activity, ActivityDto>().ReverseMap();

        CreateMap<ActivityCreateDto, Activity>();
        CreateMap<ActivityUpdateDto, Activity>()
            .ForMember(dest => dest.StartDate, opt => opt.Condition(src => src.StartDate.HasValue))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.Condition(src => src.EndDate.HasValue))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));
        //.ForMember(dest => dest.StartDate, opt => opt.MapFrom((src, dest) => src == null ? dest.StartDate : src.StartDate));
        /* .ForMember(dest => dest.StartDate, opt => opt.Condition(src => src.StartDate != default(DateTime)))
        .ForMember(dest => dest.EndDate, opt => opt.Condition(src => src.EndDate != null))
        .ForAllMembers(opt => opt.Condition((src, dest, member) => member != null)); */

        /*
         * USER
         ***********/
        CreateMap<User, User>();
        CreateMap<User, UserDto>().ReverseMap();

        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>();
    }
}
