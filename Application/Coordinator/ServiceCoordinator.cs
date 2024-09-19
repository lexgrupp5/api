using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Coordinator;

public class ServiceCoordinator(
    Lazy<ICourseService> courseService,
    Lazy<IIdentityService> identityService,
    Lazy<IModuleService> moduleService,
    Lazy<UserManager<User>> userManager
) : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService = courseService;

    private readonly Lazy<IIdentityService> _identityService = identityService;

    private readonly Lazy<IModuleService> _moduleService = moduleService;

    private readonly Lazy<UserManager<User>> _userManager = userManager;

    public ICourseService Course => _courseService.Value;
    
    public IIdentityService Identity => _identityService.Value;

    public IModuleService ModuleService => _moduleService.Value;

    public UserManager<User> User => _userManager.Value;
}
