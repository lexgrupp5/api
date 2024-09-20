using Application.Interfaces;
using Application.Models;

using AutoMapper;
using Data;
using Domain.DTOs;
using Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        //UoW
        private readonly IDataCoordinator _dataCoordinator;
        //Mapper
        private readonly IMapper _mapper;

        public UserService(IDataCoordinator dataCoordinator, IMapper mapper)
        {
            _dataCoordinator = dataCoordinator;
            _mapper = mapper;
        }

        public async Task <IEnumerable<UserDto?>> GetUsersOfCourseByIdAsync(int courseId)
        {
            var users = await _dataCoordinator.Users.GetUsersFromCourseByIdAsync(courseId);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

            return userDtos;
        }

        public async Task<UserDto?> CreateNewUserAsync(UserForCreationDto newUser, UserManager<User> userManager, IIdentityService identityService)
        {

            //var userCreateModel = _mapper.Map<UserCreateModel>(newUser);
            UserCreateModel userCreateModel = new UserCreateModel(newUser.Name, newUser.Username, newUser.Email, "Qwerty1234");
            var result = await identityService.CreateUserAsync(userCreateModel);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("\n", result.Errors));
            }
            var createdUser = await _dataCoordinator.Users.GetByConditionAsync(u => u.Name == newUser.Name).FirstAsync();
            var finalUser = await _dataCoordinator.Users.CreateNewUserAsync(createdUser);
            var finalDto = _mapper.Map<UserDto>(finalUser);
            return finalDto;

            //var userToBeCreated = _mapper.Map<User>(newUser);
            //var createdUser = await _dataCoordinator.Users.CreateNewUserAsync(userToBeCreated, userManager);
            //if (createdUser == null) { return null; }
            //var createdUserDto = _mapper.Map<UserDto>(createdUser);
            //return createdUserDto;
        }
    }
}
