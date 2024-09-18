using Application.Interfaces;

using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class ServiceCoordinator : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService;

    public ICourseService CourseService => _courseService.Value;

    private readonly Lazy<UserManager<User>> _userManager;

    public UserManager<User> User => _userManager.Value;

    public ServiceCoordinator(Lazy<ICourseService> courseService, Lazy<UserManager<User>> userManager)
    {
        _courseService = courseService;
        _userManager = userManager;
    }
}