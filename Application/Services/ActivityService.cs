using System.Linq.Expressions;
using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ActivityService : ServiceBase<Activity>, IActivityService
{
    private readonly IDataCoordinator _dc;
    private readonly IMapper _mapper;

    public ActivityService(IDataCoordinator dataCoordinator, IMapper autoMapper)
    {
        _dc = dataCoordinator;
        _mapper = autoMapper;
    }

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
            .ProjectTo<ActivityDto>(_dc.Activities.GetQuery(filters, sorting, paging))
            .ToListAsync();
    }

    /*
     *
     ****/
    public async Task<ActivityDto?> GetActivityByIdAsync<TDto>(int id)
    {
        return await _mapper
            .ProjectTo<ActivityDto>(_dc.Activities.GetQuery([a => a.Id == id]))
            .FirstOrDefaultAsync();
    }
}
