using Application.Interfaces;
using Application.Services;
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

    public ICourseService Course => _courseService.Value;
    
    public IIdentityService Identity => _identityService.Value;

    public IUserService UserService => _userService.Value;
    public IModuleService ModuleService => _moduleService.Value;

    public UserManager<User> User { get; } = userManager;
}
