using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId);
    Task<User?> CreateNewUserAsync(User? newUser);
    Task<bool> CheckUsernameExistsAsync(User user);
}