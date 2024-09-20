using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.JsonPatch;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto?>> GetUsersOfCourseByIdAsync(int courseId);
        Task<UserDto> PatchUser(User user, JsonPatchDocument<UserForUpdateDto> patchDocument);
        Task<User?> GetUserByUsername(string name);
    }
}
