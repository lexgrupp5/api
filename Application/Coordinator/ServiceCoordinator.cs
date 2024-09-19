using Application.Interfaces;
using Application.Services;

using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Application.Coordinator;

public class ServiceCoordinator(
    Lazy<ICourseService> courseService,
    Lazy<UserManager<User>> userManager,
    Lazy<IIdentityService> identityService
) : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService = courseService;

    private readonly Lazy<UserManager<User>> _userManager = userManager;

    private readonly Lazy<IIdentityService> _identityService = identityService;

    public ICourseService Course => _courseService.Value;

    public UserManager<User> User => _userManager.Value;

    public IIdentityService Identity => _identityService.Value;
}
