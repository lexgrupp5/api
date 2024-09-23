using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IServiceCoordinator
{
    ICourseService Course { get; }

    IIdentityService Identity { get; }

    IModuleService ModuleService { get; }
   
    IUserService UserService { get; }
}
