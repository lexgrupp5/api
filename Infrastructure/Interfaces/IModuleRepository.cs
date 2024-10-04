using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module>
{

    /* DEPRECATED
     **********************************************************************/

    /*
    Task<bool> CheckModuleExistsAsync(Module module);

    Task<Module?> GetModuleByIdWithActivitiesAsync(int id);

    Task<Module> GetModule(int id);
    
    Task<Module> CreateModuleAsync(ModuleCreateDto createDto);

    Task<Activity?> CreateActivityAsync(ActivityCreateDto createDto);

    Task<Activity> GetActivityByIdAsync(int id);

    IQueryable<Module> QueryModuleById(int id);
    */
}
