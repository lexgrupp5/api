using Domain.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IModuleService : IServiceBase<Module, ModuleDto>
{
    Task<IEnumerable<ActivityDto>?> GetActivitiesByModuleIdAsync(int id);
    Task<ModuleDto?> FindAsync(int id);
    Task<ModuleDto?> CreateAsync(ModuleCreateDto dto);
    Task<ModuleDto?> UpdateAsync(int id, ModuleUpdateDto dto);

    /* DEPRECATED
     ********************************************************/
    Task<ModuleDto> PatchModule(ModuleDto dto);

    //Task<ModuleForCreationDto> CreateModuleAsync(ModuleCreateModel module);
    /* Task<ActivityForCreationDto> CreateActivityAsync(ActivityCreateDto activityCreate);
    Task<ActivityDto> GetActivityByIdAsync(int id);
    Task PatchModule(ModuleDto moduleToPatchDto);
    Task PatchActivity(ActivityDto activityDto);
    Task<TDto?> GetModuleByIdAsync<TDto>(int id);
    Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id); */
}
