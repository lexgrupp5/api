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
    Task<IEnumerable<UserRefreshToken>> GetUserRefreshTokensAsync(User user);
    EntityEntry<UserRefreshToken> AddUserRefreshToken(UserRefreshToken token);
    EntityEntry<UserRefreshToken> RemoveUserRefreshToken(UserRefreshToken token);
}
