using Application.Models;

using Domain.DTOs;

namespace Application.Interfaces;

public interface IModuleService
{
    Task<ActivityForCreationDto> CreateActivityAsync(ActivityCreateModel activityCreate);
    Task<ModuleForCreationDto> CreateModuleAsync(ModuleCreateModel module);
    Task<ActivityDto> GetActivityByIdAsync(int id);
    Task<ModuleToPatchDto> GetModule(int id);
    Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id);
    Task<IEnumerable<ModuleDto?>> GetModulesByCourseIdAsync(int id);
    Task PatchActivity(ActivityDto activityDto);
    Task PatchModule(ModuleToPatchDto moduleToPatchDto);
}