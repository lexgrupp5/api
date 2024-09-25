using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Coordinator;

public class ServiceCoordinator(
    Lazy<ICourseService> courseService,
    Lazy<IIdentityService> identityService,
    Lazy<IUserService> userService,
    Lazy<IModuleService> moduleService,
    UserManager<User> userManager
) : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService = courseService;

    private readonly Lazy<IIdentityService> _identityService = identityService;

    private readonly Lazy<IUserService> _userService = userService;

    private readonly Lazy<IModuleService> _moduleService = moduleService;

    private readonly UserManager<User> _userManager = userManager;

    public ICourseService Course => _courseService.Value;

    public IIdentityService Identity => _identityService.Value;

    public IUserService UserService => _userService.Value;

    public IModuleService Module => _moduleService.Value;

    public UserManager<User> User => _userManager;
}
