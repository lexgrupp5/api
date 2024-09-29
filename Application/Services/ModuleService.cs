using Application.Interfaces;
using Application.Models;

using AutoMapper;

using Domain.DTOs;
using Domain.Entities;

using Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

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
    
    public async Task<IEnumerable<ModuleDto?>> GetModulesOfCourseIdAsync(int id, SearchFilterDTO searchFilterDto) 
    {
        var modules = await _dataCoordinator.Modules.GetModulesOfCourseAsync(id, searchFilterDto);
        var moduleDtos = _mapper.Map<IEnumerable<ModuleDto>>(modules);
        return moduleDtos;
    }

    public async Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id)
    {
        var modules = await _dataCoordinator.Modules.GetModuleByIdWithActivitiesAsync(id);
        var moduleDto = _mapper.Map<ModuleDto>(modules);
        return moduleDto;
    }

    public async Task<ModuleToPatchDto> GetModule(int id)
    {
        var module = await _dataCoordinator.Modules.GetModule(id);
        var moduleToPatchDto = _mapper.Map<ModuleToPatchDto>(module);
        return moduleToPatchDto;
    }

    public async Task<ModuleForCreationDto> CreateModuleAsync(ModuleCreateModel moduleToCreate)
    {
        var createdModule = await _dataCoordinator.Modules.CreateModuleAsync(
            _mapper.Map<ModuleForCreationDto>(moduleToCreate)
        );
        var mappedModule = new ModuleForCreationDto
        {
            Name = createdModule.Name,
            CourseId = createdModule.CourseId,
            Description = createdModule.Description,
            StartDate = createdModule.StartDate,
            EndDate = createdModule.EndDate
        };

        return mappedModule;
    }

    public async Task<ActivityForCreationDto> CreateActivityAsync(
        ActivityCreateModel activityCreate
    )
    {
        var createdActivity = await _dataCoordinator.Modules.CreateActivityAsync(
            (_mapper.Map<ActivityForCreationDto>(activityCreate))
        );
        var mappedActivity = new ActivityForCreationDto
        {
            ModuleId = createdActivity.ModuleId,
            Description = createdActivity.Description,
            StartDate = createdActivity.StartDate,
            EndDate = createdActivity.EndDate
        };
        return mappedActivity;
    }

    public async Task PatchModule(ModuleToPatchDto moduleToPatchDto)
    {
        var module = await _dataCoordinator
            .Modules.GetByConditionAsync(module => module.Id == moduleToPatchDto.Id)
            .FirstOrDefaultAsync();

        if (module == null)
        {
            NotFound($"Module with the ID {moduleToPatchDto.Id} was not found in the database.");
        }

        _mapper.Map(moduleToPatchDto, module);

        await _dataCoordinator.CompleteAsync();
    }

    public async Task PatchActivity(ActivityDto activityDto)
    {
        var activity = await _dataCoordinator.Modules.GetActivityByIdAsync(activityDto.Id);

        if (activity == null)
        {
            NotFound($"Activity with the ID {activityDto.Id} was not found in the database.");
        }

        _mapper.Map(activityDto, activity);

        await _dataCoordinator.CompleteAsync();
    }

    public async Task<ActivityDto> GetActivityByIdAsync(int id)
    {
        var activity = await _dataCoordinator.Modules.GetActivityByIdAsync(id);
        var activityDto = _mapper.Map<ActivityDto>(activity);
        return activityDto;
    }
}
