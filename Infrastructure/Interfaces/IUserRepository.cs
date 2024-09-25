using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId);
    Task<User?> GetUserByUsername(string username);
    Task<bool> CheckUsernameExistsAsync(User user);
    Task<User?> CreateNewUserAsync(User? newUser);

    //Refresh Tokens
    Task<UserSession?> GetUserSessionAsync(string token);
    Task<IEnumerable<UserSession>> GetAllUserSessionsAsync(User user);
    Task<User?> GetUserSessionByTokenAsync(string token);
    EntityEntry<UserSession> AddUserSession(UserSession token);
    EntityEntry<UserSession> RemoveUserSession(UserSession token);
}
