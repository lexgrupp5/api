using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>?> GetUsersAsync();

    /* DEPRECATED
     **********************************************************************/

    Task<UserDto?> PatchUser(string username, JsonPatchDocument<UserForUpdateDto> patchDocument);
    Task<UserDto?> CreateNewUserAsync(
        UserForCreationDto newUser,
        UserManager<User> userManager,
        IIdentityService identityService
    );
}
