using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(AppDbContext context)
        : base(context)
    {
        _db = context;
    }

    public async Task<IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId)
    {
        var course = await _db.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync();
        if (course == null)
        {
            return null;
        }
        var students = await _db.Users.Where(u => u.CourseId == courseId).ToListAsync();
        return students;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        //var user = await GetByConditionAsync(u => u.Name.Replace(" ", "").ToUpper() ==  username.ToUpper()).FirstAsync();
        var user = await _db
            .Users.Where(u => u.Name.Replace(" ", "").ToUpper() == username.ToUpper())
            .FirstOrDefaultAsync();
        Console.WriteLine("Taking up space");
        if (user == null)
        {
            return null;
        }
        return user;
    }

    public async Task<User?> CreateNewUserAsync(User? newUser)
    {
        if (newUser == null)
        {
            throw new ArgumentNullException(nameof(newUser));
        }
        await _db.SaveChangesAsync();
        return newUser;
    }

    public async Task<bool> CheckUsernameExistsAsync(User user)
    {
        return await _db.Users.AnyAsync(u => u.Name == user.Name);
    }

    public async Task<IEnumerable<UserRefreshToken>> GetUserRefreshTokensAsync(User user) =>
        await _db.RefreshTokens.Where(r => r.User == user).ToListAsync();

    public EntityEntry<UserRefreshToken> AddUserRefreshToken(UserRefreshToken token) =>
        _db.RefreshTokens.Add(token);

    public EntityEntry<UserRefreshToken> RemoveUserRefreshToken(UserRefreshToken token) =>
        _db.RefreshTokens.Remove(token);
}
