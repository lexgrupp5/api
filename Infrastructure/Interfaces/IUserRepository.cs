using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId);
    Task<User?> GetUserByUsername(string username);
    Task<bool> CheckUsernameExistsAsync(User user);
    Task<User?> CreateNewUserAsync(User? newUser);
}