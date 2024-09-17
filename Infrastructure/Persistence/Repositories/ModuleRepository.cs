using Domain.Entities;

using Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
{
    public ModuleRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckModuleExistsAsync(Module module)
    {
        return await _context.Modules.AnyAsync(m => m.Name == module.Name);
    }
}