using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> CheckUsernameExistsAsync(User user);
}