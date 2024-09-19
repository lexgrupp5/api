using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class ServiceCoordinator : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService;
    private readonly Lazy<IModuleService> _moduleService;
    private readonly Lazy<IUserService> _userService;

    public ICourseService CourseService => _courseService.Value;
    public IModuleService ModuleService => _moduleService.Value;
    public IUserService UserService => _userService.Value;

    public ServiceCoordinator(Lazy<ICourseService> courseService,Lazy<IModuleService> moduleService ,Lazy<IUserService> userService)
    {
        _courseService = courseService;
        _moduleService = moduleService;
        _userService = userService;
    }
}