using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IServiceCoordinator
{
    ICourseService CourseService { get; }
    IModuleService ModuleService { get; }
    UserManager<User> User { get; }
}