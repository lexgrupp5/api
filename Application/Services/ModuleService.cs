using Application.Interfaces;

using Domain.DTOs;

using Infrastructure.Interfaces;

namespace Application.Services;

public class ModuleService(IDataCoordinator dataCoordinator) : IModuleService
{
    public async Task<ModuleDto?> GetModuleAsync(int id)
    {
        return await dataCoordinator.Modules.GetModuleByIdAsync(id);
    }
}