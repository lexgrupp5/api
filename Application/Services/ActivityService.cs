using System.Linq.Expressions;
using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ActivityService : ServiceBase<Activity, ActivityDto>, IActivityService
{
    public ActivityService(IDataCoordinator dataCoordinator, IMapper autoMapper)
        : base(dataCoordinator, autoMapper) { }

    /*
     *
     ****/
    public async Task<IEnumerable<ActivityDto?>> GetActivitiesAsync(
        ICollection<Expression<Func<Activity, bool>>>? filters = null,
        ICollection<SortParams>? sorting = null,
        PageParams? paging = null
    )
    {
        return await _mapper
            .ProjectTo<ActivityDto>(_data.Activities.GetQuery(filters, sorting, paging))
            .ToListAsync();
    }

    /*
     *
     ****/
    public async Task<ActivityDto?> GetActivityByIdAsync<TDto>(int id)
    {
        return await _mapper
            .ProjectTo<ActivityDto>(_data.Activities.GetQuery([a => a.Id == id]))
            .FirstOrDefaultAsync();
    }

    public async Task<ActivityDto> CreateAsync(ActivityCreateDto createDto)
    {
        if (createDto.StartDate > createDto.EndDate)
            BadRequest("Start date must be before end date");

        var module = await _mapper
            .ProjectTo<Module>(_data.Modules.GetQueryById(createDto.ModuleId))
            .FirstOrDefaultAsync();
        if (module == null)
            NotFound($"Module with the ID {createDto.ModuleId} was not found in the database.");

        var newActivity = _mapper.Map<Activity>(createDto);

        ValidateActivityDates(
            newActivity,
            module,
            [.. module.Activities],
            [.. module.Course.Modules]
        );

        await _data.Activities.AddAsync(newActivity);
        await _data.CompleteAsync();
        return _mapper.Map<ActivityDto>(newActivity);
    }

    public async Task<ActivityDto> UpdateAsync(int id, ActivityUpdateDto updateDto)
    {
        if (updateDto.Id != id)
            BadRequest("Id missmatch");

        var current = await _mapper
            .ProjectTo<Activity>(_data.Activities.GetQueryById(id))
            .FirstOrDefaultAsync();
        if (current == null)
            NotFound();

        _mapper.Map(updateDto, current);
        ValidateActivityDates(
            current,
            current.Module,
            [.. current.Module.Activities],
            [.. current.Module.Course.Modules]
        );
        await _data.CompleteAsync();
        return _mapper.Map<ActivityDto>(current);
    }

    /* PRIVATE HELPERS
     **********************************************************************/

    private static bool ValidateActivityDates(
        Activity activity,
        Module module,
        List<Activity> moduleActivities,
        List<Module> courseModules
    )
    {
        var isModuleStartOlder = module.StartDate > activity.StartDate;
        var isModuleEndNewer = module.EndDate < activity.EndDate;

        if (isModuleStartOlder || isModuleEndNewer)
        {
            if (courseModules.Count > 1)
            {
                var moduleIndex = courseModules.IndexOf(module);
                if (isModuleStartOlder && moduleIndex > 1)
                {
                    if (courseModules[moduleIndex - 1].EndDate > activity.StartDate)
                    {
                        BadRequest("Module start date conflict");
                    }
                }
                if (isModuleEndNewer && moduleIndex < courseModules.Count - 1)
                {
                    if (courseModules[moduleIndex + 1].StartDate < activity.EndDate)
                    {
                        BadRequest("Module end date conflict");
                    }
                }
            }
        }

        var activities = moduleActivities.OrderBy(a => a.EndDate).ToList();

        if (activities.Count > 0)
        {
            var isBeforeFirst = activities.First().StartDate > activity.EndDate;
            var isAfterLast = activities.Last().EndDate < activity.StartDate;

            if (!isBeforeFirst && !isAfterLast)
            {
                var index = activities.FindIndex(a => a.EndDate < activity.StartDate);
                if ((index > 0) && (activities[index + 1].StartDate > activity.EndDate)) { }
                else
                {
                    BadRequest("Activity date range overlap conflict");
                }
            }
        }
        return true;
    }

    /* DEPRECATED
     **********************************************************************/

    public async Task<ActivityDto> PatchActivity(ActivityDto dto)
    {
        var current = await _data.Activities.FindAsync(dto.Id);
        if (current == null)
            NotFound($"Activity with the ID {dto.Id} was not found in the database.");

        _mapper.Map(dto, current);
        await _data.CompleteAsync();
        return _mapper.Map<ActivityDto>(current);
    }
}
