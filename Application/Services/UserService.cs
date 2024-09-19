using Application.Interfaces;

using Data;

using Domain.DTOs;

namespace Application.Services;

public class UserService : IUserService
{
    //UoW
    private readonly IDataCoordinator _dataCoordinator;
    
    public UserService(IDataCoordinator dataCoordinator)
    {
        _dataCoordinator = dataCoordinator;
    }
    
    public async Task<IEnumerable<UserDto>> GetAllStudentsAsync()
    {
       return await _dataCoordinator.Users.GetAllStudentsAsync();
    }
    
}