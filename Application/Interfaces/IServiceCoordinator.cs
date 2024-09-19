using Domain.Entities;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Service;

public interface IServiceCoordinator
{
    ICourseService CourseService { get; }
    IModuleService ModuleService { get; }
    UserManager<User> User { get; }
}
