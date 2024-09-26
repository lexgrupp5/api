using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IServiceCoordinator
{
    ICourseService Course { get; }

    IIdentityService Identity { get; }

    IModuleService Module { get; }
   
    IUserService UserService { get; }

    UserManager<User> User { get; }
}
