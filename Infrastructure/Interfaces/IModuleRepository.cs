using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module>
{
    Task<ICollection<Module>?> GetModulesByCourseIdAsync(int id);

    Task<bool> CheckModuleExistsAsync(Module module);

    Task<Module?> GetModuleByIdWithActivitiesAsync(int id);

    Task<Module> CreateModuleAsync(ModuleForCreationDto moduleToCreate);

    Task<Activity?> CreateActivityAsync(ActivityForCreationDto activityToCreate); 

}