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
    private readonly IDataCoordinator _dc;
    //Mapper
    private readonly IMapper _mapper;

    public ModuleService(IDataCoordinator dataCoordinator, IMapper mapper)
    {
        _dc = dataCoordinator;
        _mapper = mapper;
    }

    public async Task<TDto?> GetModuleByIdAsync<TDto>(int id)
    {
        var queryResult = await _dc.Modules.QueryModuleById(id)
            .Include(c => c.Course)
            .Include(m => m.Activities)
            .ThenInclude(a => a.ActivityType)
            .FirstOrDefaultAsync();
        var result = _mapper.Map<TDto>(queryResult);
        return result;
    }

    public async Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id)
    {
        var modules = await _dc.Modules.GetModuleByIdWithActivitiesAsync(id);
        var moduleDto = _mapper.Map<ModuleDto>(modules);
        return moduleDto;
    }

    public async Task<ModuleToPatchDto> GetModule(int id)
    {
        var module = await _dc.Modules.GetModule(id);
        var moduleToPatchDto = _mapper.Map<ModuleToPatchDto>(module);
        return moduleToPatchDto;
    }

    public async Task<ModuleForCreationDto> CreateModuleAsync(ModuleCreateModel moduleToCreate)
    {
        var createdModule = await _dc.Modules.CreateModuleAsync(
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
        var createdActivity = await _dc.Modules.CreateActivityAsync(
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
        var module = await _dc
            .Modules.GetByConditionAsync(module => module.Id == moduleToPatchDto.Id)
            .FirstOrDefaultAsync();

        if (module == null)
        {
            NotFound($"Module with the ID {moduleToPatchDto.Id} was not found in the database.");
        }

        _mapper.Map(moduleToPatchDto, module);

        await _dc.CompleteAsync();
    }

    public async Task PatchActivity(ActivityDto activityDto)
    {
        var activity = await _dc.Modules.GetActivityByIdAsync(activityDto.Id);

        if (activity == null)
        {
            NotFound($"Activity with the ID {activityDto.Id} was not found in the database.");
        }

        _mapper.Map(activityDto, activity);

        await _dc.CompleteAsync();
    }

    public async Task<ActivityDto> GetActivityByIdAsync(int id)
    {
        var activity = await _dc.Modules.GetActivityByIdAsync(id);
        var activityDto = _mapper.Map<ActivityDto>(activity);
        return activityDto;
    }
}
