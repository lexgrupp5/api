using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IServiceCoordinator
{
    ICourseService Course { get; }
    UserManager<User> User { get; }
    IIdentityService Identity { get; }
}
