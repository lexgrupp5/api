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

    public async Task<bool> CheckModuleExistsAsync(Module module)
    {
        return await _context.Modules.AnyAsync(m => m.Name == module.Name);
    }

    public async Task<ModuleDto?> GetModuleByIdAsync(int id)
    {
        var result = await GetByConditionAsync(m => m.Id.Equals(id)).Include(m => m.Activities).FirstOrDefaultAsync();
        return result == null ? null : _mapper.Map<ModuleDto>(result);
    }
}