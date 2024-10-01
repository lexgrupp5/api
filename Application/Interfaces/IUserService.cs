using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto?>> GetUsersOfCourseByIdAsync(int courseId);
    Task<UserDto?> PatchUser(string username, JsonPatchDocument<UserForUpdateDto> patchDocument);
    Task<User?> GetUserByUsername(string name);
    Task<UserDto?> CreateNewUserAsync(
        UserForCreationDto newUser,
        UserManager<User> userManager,
        IIdentityService identityService
    );
    Task<IEnumerable<UserDto>> TestUserQuery();
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
}
