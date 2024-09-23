using Application.Interfaces;

namespace Application.Coordinator;

public class ServiceCoordinator(
    Lazy<ICourseService> courseService,
    Lazy<IIdentityService> identityService,
    Lazy<IUserService> userService,
    Lazy<IModuleService> moduleService
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

}
