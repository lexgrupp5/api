using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module>
{
    Task<bool> CheckModuleExistsAsync(Module module);
}