using Application.Interfaces;
using AutoMapper;
using Data;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : ServiceBase<User>, IUserService
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
    }
}
