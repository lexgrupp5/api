using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService : ServiceBase<User, UserDto>, IUserService
{
    public UserService(IDataCoordinator dataCoordinator, IMapper mapper)
        : base(dataCoordinator, mapper) { }

    public async Task<IEnumerable<UserDto>?> GetUsersAsync()
    {
        var users = await _data.Users.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> UpdateAsync(UserUpdateDto dto)
    {
        var current = await FindUserAsync(dto.Username);
        if (current == null)
            NotFound();

        _mapper.Map(dto, current);
        await _data.CompleteAsync();
        return _mapper.Map<UserDto>(current);
    }

    /* PRIVATE HELPERS
     **********************************************************************/

    private async Task<User?> FindUserAsync(string username) => await FindUserAsync<User>(username);

    private async Task<T?> FindUserAsync<T>(string username) =>
        await _mapper
            .ProjectTo<T>(_data.Users.GetQuery([u => u.UserName == username]))
            .FirstOrDefaultAsync();

    /* DEPRECATED
     **********************************************************************/

    public async Task<UserDto?> PatchUser(
        string username,
        JsonPatchDocument<UserForUpdateDto> patchDocument
    )
    {
        var currentUser = await _data
            .Users.GetQuery([u => u.UserName == username])
            .FirstOrDefaultAsync();
        if (currentUser == null)
        {
            return null;
        }

        var userToPatch = _mapper.Map<UserForUpdateDto>(currentUser);
        patchDocument.ApplyTo(userToPatch);

        currentUser.Name = userToPatch.Name;
        currentUser.Email = userToPatch.Email;
        currentUser.UserName = userToPatch.Username;
        currentUser.Course = userToPatch.Course;
        await _data.CompleteAsync();

        var updatedUser = _mapper.Map<UserDto>(currentUser);
        return updatedUser;
    }

    public async Task<UserDto?> CreateNewUserAsync(
        UserForCreationDto newUser,
        UserManager<User> userManager,
        IIdentityService identityService
    )
    {
        UserCreateModel userCreateModel = new UserCreateModel(
            newUser.Name,
            newUser.Username,
            newUser.Email,
            "Qwerty1234"
        );

        var user = _mapper.Map<User>(userCreateModel);
        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join("\n", result.Errors));
        }
        var createdUser = await _data
            .Users.GetByConditionAsync(u => u.Name == newUser.Name)
            .FirstAsync();
        var finalUser = await _data.Users.CreateNewUserAsync(createdUser);
        var finalDto = _mapper.Map<UserDto>(finalUser);
        return finalDto;
    }
}
