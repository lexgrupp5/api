using Application.Interfaces;

using AutoMapper;

using Domain.DTOs;

using Infrastructure.Interfaces;

namespace Application.Services;

public class ModuleService : IModuleService
{
    //UoW
    private readonly IDataCoordinator _dataCoordinator;

    //Mapper
    private readonly IMapper _mapper;

    public ModuleService(IDataCoordinator dataCoordinator, IMapper mapper)
    {
        _dataCoordinator = dataCoordinator;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ModuleDto?>> GetModulesOfCourseIdAsync(int id)
    {
        var modules = await _dataCoordinator.Modules.GetModulesOfCourseAsync(id);
        var moduleDtos = _mapper.Map<IEnumerable<ModuleDto>>(modules);
        return moduleDtos;
    }

    public async Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id)
    {
        var modules = await _dataCoordinator.Modules.GetModuleByIdWithActivitiesAsync(id);
        var moduleDto = _mapper.Map<ModuleDto>(modules);
        return moduleDto;
    }
}