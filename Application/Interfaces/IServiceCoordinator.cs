using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IServiceCoordinator
{
    IActivityService Activity { get; }
    ICourseService Course { get; }
    IIdentityService Identity { get; }
    IModuleService Module { get; }
    IUserService User { get; }
    UserManager<User> UserManager { get; }
}
