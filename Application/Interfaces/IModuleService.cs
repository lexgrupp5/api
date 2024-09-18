using Domain.DTOs;

namespace Application.Interfaces;

public interface IModuleService
{
    Task<ModuleDto?> GetModuleAsync(int id);
}