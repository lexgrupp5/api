using Domain.DTOs;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto?>> GetUsersOfCourseByIdAsync(int courseId);
    }
}
