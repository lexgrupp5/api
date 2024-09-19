using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class ServiceCoordinator : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<IModuleService> _moduleService;
    private readonly Lazy<IUserService> _userService;
    private readonly UserManager<User> _userManager;

    public ICourseService CourseService => _courseService.Value;
    public IModuleService ModuleService => _moduleService.Value;
    public IUserService UserService => _userService.Value;
    public UserManager<User> User => _userManager;

    public ServiceCoordinator(Lazy<ICourseService> courseService,Lazy<IModuleService> moduleService ,UserManager<User> userManager, Lazy<IUserService> userService)
    {
        _courseService = courseService;
        _moduleService = moduleService;
        _userService = userService;
        _userManager = userManager;
    }
}