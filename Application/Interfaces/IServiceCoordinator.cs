using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;

namespace Service;

public interface IServiceCoordinator
{
    ICourseService CourseService { get; }
    UserManager<User> User { get; }
}