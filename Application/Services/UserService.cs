using Application.Interfaces;
using AutoMapper;
using Data;
using Domain.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

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

        public async Task<User?> GetUserByUsername(string name)
        {
            return await _dataCoordinator.Users.GetUserByUsername(name);
        }

        public async Task<UserDto> PatchUser(User user, JsonPatchDocument<UserForUpdateDto> patchDocument)
        {
            var userToPatch = _mapper.Map<UserForUpdateDto>(user);
            patchDocument.ApplyTo(userToPatch);

            user.Name = userToPatch.Name;
            user.Email = userToPatch.Email;
            user.UserName = userToPatch.Username;
            user.Course = userToPatch.Course;
            await _dataCoordinator.CompleteAsync();
            
            var updatedUser = _mapper.Map<UserDto>(user);
            return updatedUser;
        }
    }
}
