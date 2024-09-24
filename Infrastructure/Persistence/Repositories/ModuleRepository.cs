using AutoMapper;

using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    private readonly IMapper _mapper;
    public ModuleRepository(AppDbContext context, IMapper mapper) : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Module?>?> GetModulesOfCourseAsync(int id)
    {
        var course = await _context.Courses.Where(c => c.Id.Equals(id)).Include(c => c.Modules).FirstOrDefaultAsync();
        if (course == null) { return null; }

        return course.Modules.ToList();
    }

    public async Task<Module?> GetModuleByIdWithActivitiesAsync(int id)
    {
        var result = await GetByConditionAsync(m => m.Id.Equals(id)).Include(m => m.Activities).FirstOrDefaultAsync();
        return result;
    }


    public async Task<bool> CheckModuleExistsAsync(Module module)
    {
        return await _context.Modules.AnyAsync(m => m.Name == module.Name);
    }


    public async Task<Module> CreateModuleAsync(ModuleForCreationDto moduleToCreate)
    {
        ArgumentNullException.ThrowIfNull(moduleToCreate);
        var newModule = _mapper.Map<Module>(moduleToCreate);
        _context.Modules.Add(newModule);
        await _context.SaveChangesAsync();
        return newModule;
    }
}