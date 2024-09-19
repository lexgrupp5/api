using Domain.DTOs;

namespace Application.Interfaces
{
    public interface IModuleService
    {
        Task<IEnumerable<ModuleDto?>> GetModulesOfCourseIdAsync(int id);
      
        Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id);
    }
}
