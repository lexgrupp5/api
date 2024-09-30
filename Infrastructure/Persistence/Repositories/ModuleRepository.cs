using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ModuleRepository(AppDbContext context, IMapper mapper)
    : RepositoryBase<Module>(context),
        IModuleRepository
{
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Module?>?> GetModulesOfCourseAsync(int id, SearchFilterDTO searchFilterDto)
    {
        IQueryable<Module> query = _db.Courses.Where(x => x.Id == id)
            .Include(x => x.Modules)
            .SelectMany(x => x.Modules);

        if (searchFilterDto.SearchText != null)
        {
            query = query.Where(m => m.Name.Contains(searchFilterDto.SearchText));
        }

        var modules = await query.ToListAsync();

        return modules;
    }

    public async Task<Module?> GetModuleByIdWithActivitiesAsync(int id) =>
        await GetByConditionAsync(m => m.Id.Equals(id))
            .Include(m => m.Activities)
            .FirstOrDefaultAsync();

    public async Task<Module> GetModule(int id)
    {
        var result = await GetByConditionAsync(m => m.Id.Equals(id)).FirstOrDefaultAsync();
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

    public Task<Module?> GetModuleByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Module> QueryModuleById(int id) => _db.Modules.Where(x => x.Id == id);
}
