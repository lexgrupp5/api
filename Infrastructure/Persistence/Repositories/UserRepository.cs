using Domain.Entities;

using Infrastructure.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckUsernameExistsAsync(User user)
    {
        return await _context.Users.AnyAsync(u => u.Name == user.Name);
    }
}