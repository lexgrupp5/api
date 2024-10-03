using System.Linq.Expressions;

using Domain.Entities;

using Infrastructure.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId);
    Task<bool> CheckUsernameExistsAsync(User user);
    Task<User?> CreateNewUserAsync(User? newUser);
    /* IQueryable<User> QueryUsersInRole(string roleName);
    IQueryable<User> UsersInRole(IQueryable<User> query, string roleName); */

     IQueryable<User> GetQueryUsersInRole(
        string roleName,
        IEnumerable<Expression<Func<User, bool>>> filters,
        IEnumerable<SortParams>? sorting = null,
        PageParams? paging = null
    );

    //Refresh Tokens
    Task<UserSession?> GetUserSessionAsync(string token);
    Task<IEnumerable<UserSession>> GetAllUserSessionsAsync(User user);
    Task<User?> GetUserSessionByTokenAsync(string token);
    EntityEntry<UserSession> AddUserSession(UserSession token);
    EntityEntry<UserSession> RemoveUserSession(UserSession token);
}
