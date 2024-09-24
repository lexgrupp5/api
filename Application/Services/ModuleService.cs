using Application.Interfaces;
using Application.Models;

using AutoMapper;
using Data;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Services;

public class ModuleService : ServiceBase<Module>, IModuleService
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

    public async Task<ModuleForCreationDto> CreateModuleAsync(ModuleCreateModel moduleToCreate)
    {
        var createdModule =
            await _dataCoordinator.Modules.CreateModuleAsync(_mapper.Map<ModuleForCreationDto>(moduleToCreate));
        var mappedModule = new ModuleForCreationDto
        {
            Name = createdModule.Name,
            Description = createdModule.Description,
            StartDate = createdModule.StartDate,
            EndDate = createdModule.EndDate
        };

        return mappedModule;
    }
}
