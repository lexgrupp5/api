using System.Linq.Expressions;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Configuration;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService : ServiceBase<User>, IUserService
{
    private readonly IDataCoordinator _data;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenConfig _tokenOptions;
    private readonly ITokenService _tokenService;

    public UserService(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        ITokenService tokenService,
        TokenConfig tokenOptions,
        IDataCoordinator dataCoordinator,
        IMapper mapper
    )
    {
        _data = dataCoordinator;
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
        _tokenOptions = tokenOptions;
        _tokenService = tokenService;
    }

    public async Task<UserDto?> UpdateAsync(UserForUpdateDto dto)
    {
        var current = await FindUserAsync(dto.Username);
        if (current == null)
            NotFound();

        _mapper.Map(dto, current);
        await _data.CompleteAsync();
        return _mapper.Map<UserDto>(current);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _data.Users.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
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

    private async Task<User?> FindUserAsync(string username) => await FindUserAsync<User>(username);

    private async Task<T?> FindUserAsync<T>(string username) =>
        await _mapper
            .ProjectTo<T>(_data.Users.GetQuery([u => u.UserName == username]))
            .FirstOrDefaultAsync();

    // OLD REMOVE EVENTUALLY

    public async Task<UserDto?> PatchUser(
        string username,
        JsonPatchDocument<UserForUpdateDto> patchDocument
    )
    {
        var userToBeUpdated = await GetUserByUsername(username);
        if (userToBeUpdated == null)
        {
            return null;
        }

        var userToPatch = _mapper.Map<UserForUpdateDto>(userToBeUpdated);
        patchDocument.ApplyTo(userToPatch);

        userToBeUpdated.Name = userToPatch.Name;
        userToBeUpdated.Email = userToPatch.Email;
        userToBeUpdated.UserName = userToPatch.Username;
        userToBeUpdated.Course = userToPatch.Course;
        await _data.CompleteAsync();

        var updatedUser = _mapper.Map<UserDto>(userToBeUpdated);
        return updatedUser;
    }

    public async Task<IEnumerable<UserDto>> TestUserQuery()
    {
        // Setup filters i/e where clauses
        List<Expression<Func<User, bool>>> filters =
        [
            u => u.UserName!.ToLower().Contains("test".ToLower())
        ];

        // Setup property/column sorting, order matters
        List<SortParams> sorting = [new() { Field = "Name", Descending = false }];

        // Pagination offset and size of set.
        PageParams pagination = new() { Page = 1, Size = 10 };

        // Get the queryable with filters, sorting and pagination
        var query = _data.Users.GetQuery(filters, sorting, pagination);

        // Use automapper for projection and eager loading
        return await _mapper.ProjectTo<UserDto>(query).ToListAsync();
    }

    public async Task<IEnumerable<UserDto?>> GetUsersOfCourseByIdAsync(int courseId)
    {
        var users = await _data.Users.GetUsersFromCourseByIdAsync(courseId);
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users); 

        return userDtos;
    }

    public async Task<User?> GetUserByUsername(string name) =>
        await _userManager.FindByNameAsync(name);
}
