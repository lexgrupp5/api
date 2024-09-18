using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Service;

public class ServiceCoordinator : IServiceCoordinator
{
    private readonly Lazy<ICourseService> _courseService;
    private readonly UserManager<User> _userManager;

    public ICourseService CourseService => _courseService.Value;
    public UserManager<User> User => _userManager;

    public ServiceCoordinator(Lazy<ICourseService> courseService, UserManager<User> userManager)
    {
        _courseService = courseService;
        _userManager = userManager;
    }
}