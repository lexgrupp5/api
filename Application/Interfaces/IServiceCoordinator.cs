using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Service;

public interface IServiceCoordinator
{
    UserManager<User> User { get; }
}