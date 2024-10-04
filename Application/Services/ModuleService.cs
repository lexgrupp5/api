using Application.Interfaces;

using AutoMapper;

using Domain.DTOs;
using Domain.Entities;

using Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ModuleService : ServiceBase<Module, ModuleDto>, IModuleService
{
    public ModuleService(IDataCoordinator dataCoordinator, IMapper mapper)
        : base(dataCoordinator, mapper) { }

    public async Task<ModuleDto?> FindAsync(int id)
    {
        var query = _data.Modules.GetQueryById(id);
        return await _mapper.ProjectTo<ModuleDto>(query).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ActivityDto>?> GetActivitiesByModuleIdAsync(int id)
    {
        var query = _data.Activities.GetQuery([a => a.ModuleId == id]);
        return await _mapper.ProjectTo<ActivityDto>(query).ToListAsync();
    }

    public async Task<ModuleDto?> CreateAsync(ModuleCreateDto dto)
    {
        var newModule = _mapper.Map<Module>(dto);
        await _data.Modules.AddAsync(newModule);
        await _data.CompleteAsync();

        return _mapper.Map<ModuleDto>(newModule);
    }

    public async Task<ModuleDto?> UpdateAsync(int id, ModuleUpdateDto dto)
    {
        if (id != dto.Id)
            BadRequest("Id missmatch");

        var current = await _data.Modules.FindAsync(dto.Id);
        if (current == null)
            NotFound();

        _mapper.Map(dto, current);
        await _data.CompleteAsync();

        return _mapper.Map<ModuleDto>(current);
    }

    /* DEPRECATED
     * *****************************************************************************/


    /*
     *
     ****/
    public async Task<ModuleDto> PatchModule(ModuleDto dto)
    {
        var current = await _data
            .Modules.GetByConditionAsync(module => module.Id == dto.Id)
            .FirstOrDefaultAsync();
        if (current == null)
            NotFound($"Module with the ID {dto.Id} was not found in the database.");

        _mapper.Map(dto, current);
        await _data.CompleteAsync();

        return _mapper.Map<ModuleDto>(current);
    }

    /* public async Task<ModuleForCreationDto> CreateModuleAsync(ModuleCreateModel moduleToCreate)
    {
        var createdModule = await _data.Modules.CreateModuleAsync(
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

    public async Task<TDto?> GetModuleByIdAsync<TDto>(int id)
    {
        return await _mapper.ProjectTo<TDto>(_data.Modules.QueryModuleById(id)).FirstAsync();
    }

    public async Task<ModuleDto?> GetModuleByIdWithActivitiesAsync(int id)
    {
        var modules = await _data.Modules.GetModuleByIdWithActivitiesAsync(id);
        var moduleDto = _mapper.Map<ModuleDto>(modules);
        return moduleDto;
    }

    public async Task<ActivityForCreationDto> CreateActivityAsync(ActivityCreateDto activityCreate)
    {
        var createdActivity = await _data.Modules.CreateActivityAsync(
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

    public async Task<ActivityDto> GetActivityByIdAsync(int id)
    {
        var activity = await _data.Modules.GetActivityByIdAsync(id);
        var activityDto = _mapper.Map<ActivityDto>(activity);
        return activityDto;
    }

    

     */
}
