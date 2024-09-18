using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module>
{
    Task<bool> CheckModuleExistsAsync(Module module);

    Task<ModuleDto?> GetModuleByIdAsync(int id);
}