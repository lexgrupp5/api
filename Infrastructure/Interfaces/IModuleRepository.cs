using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module>
{
    Task<IEnumerable<Module?>?> GetModulesOfCourseAsync(int id);

    Task<bool> CheckModuleExistsAsync(Module module);

    Task<Module?> GetModuleByIdWithActivitiesAsync(int id);

    Task<Module> GetModule(int id);

    Task<Module> CreateModuleAsync(ModuleForCreationDto moduleToCreate);

    Task<Activity?> CreateActivityAsync(ActivityForCreationDto activityToCreate);

    Task<Activity> GetActivityByIdAsync(int id);
}