using Application.Models;

using Domain.DTOs;

namespace Application.Interfaces;

public interface IModuleService
{
    Task<IEnumerable<ModuleDto?>> GetModulesOfCourseIdAsync(int id);
  
    Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id);
    
    Task<ActivityForCreationDto> CreateActivityAsync(ActivityCreateModel activityCreate);
}
