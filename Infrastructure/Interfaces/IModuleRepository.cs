using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module>
{
    Task<IEnumerable<Module?>?> GetModulesOfCourseAsync(int id);
    Task<bool> CheckModuleExistsAsync(Module module);

    Task<Module?> GetModuleByIdWithActivitiesAsync(int id);
}