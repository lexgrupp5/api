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

    /* DEPRECATED
     **********************************************************************/

    public async Task<ActivityDto> PatchActivity(ActivityDto dto)
    {
        var current = await _data.Modules.GetActivityByIdAsync(dto.Id);
        if (current == null)
            NotFound($"Activity with the ID {dto.Id} was not found in the database.");

        _mapper.Map(dto, current);
        await _data.CompleteAsync();
        return _mapper.Map<ActivityDto>(current);
    }
}
