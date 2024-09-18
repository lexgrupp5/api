using AutoMapper;

using Domain.DTOs;
using Domain.Entities;

namespace Application;

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
                ));
    }
}