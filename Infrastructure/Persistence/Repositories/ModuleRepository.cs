using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Persistence.Repositories;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    public ModuleRepository(AppDbContext context)
        : base(context) { }

    /* DEPRECATED
     **********************************************************************/

    /*

    public async Task<Module?> GetModuleByIdWithActivitiesAsync(int id) =>
        await GetByConditionAsync(m => m.Id.Equals(id))
            .Include(m => m.Activities)
            .FirstOrDefaultAsync();

    public async Task<Module> GetModule(int id)
    {
        var result = await GetByConditionAsync(m => m.Id.Equals(id))
            .Include(m => m.Activities)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<bool> CheckModuleExistsAsync(Module module) =>
        await _db.Modules.AnyAsync(m => m.Name == module.Name);

    public async Task<Module> CreateModuleAsync(ModuleForCreationDto moduleToCreate)
    {
        ArgumentNullException.ThrowIfNull(moduleToCreate);
        var newModule = _mapper.Map<Module>(moduleToCreate);
        _db.Modules.Add(newModule);
        await _db.SaveChangesAsync();
        return newModule;
    }

    public async Task<Activity?> CreateActivityAsync(ActivityForCreationDto activityToCreate)
    {
        var newActivity = _mapper.Map<Activity>(activityToCreate);
        _db.Activities.Add(newActivity);
        await _db.SaveChangesAsync();
        return newActivity;
    }

    public async Task<Activity> GetActivityByIdAsync(int id)
    {
        var activity = await _db.Activities.FirstOrDefaultAsync(x => x.Id == id);
        return activity;
    }

    public IQueryable<Module> QueryModuleById(int id) => _db.Modules.Where(x => x.Id == id);

    */
}
