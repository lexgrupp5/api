using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Coordinator;

public class ServiceCoordinator : IServiceCoordinator
{
    private readonly Lazy<IActivityService> _activityService;
    private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<IIdentityService> _identityService;
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IModuleService> _moduleService;

    public ServiceCoordinator(
        Lazy<IActivityService> activityService,
        Lazy<ICourseService> courseService,
        Lazy<IIdentityService> identityService,
        Lazy<IUserService> userService,
        Lazy<IModuleService> moduleService,
        UserManager<User> userManager
    )
    {
        _activityService = activityService;
        _courseService = courseService;
        _identityService = identityService;
        _userService = userService;
        _moduleService = moduleService;
        UserManager = userManager;
    }

    public IActivityService Activity => _activityService.Value;
    public ICourseService Course => _courseService.Value;
    public IIdentityService Identity => _identityService.Value;
    public IUserService User => _userService.Value;
    public IModuleService Module => _moduleService.Value;
    public UserManager<User> UserManager { get; private set; }
}
