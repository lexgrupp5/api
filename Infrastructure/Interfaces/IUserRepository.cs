using Domain.DTOs;
using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId);
    Task<bool> CheckUsernameExistsAsync(User user);
    
}