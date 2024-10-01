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

    // UserSession

    /*
     *
     ****/
    public async Task<UserSession?> GetUserSessionAsync(string token) =>
        await _db
            .UserSession.Where(x => x.RefreshToken == token)
            .Include(x => x.User)
            .FirstOrDefaultAsync();

    /*
     *
     ****/
    public async Task<IEnumerable<UserSession>> GetAllUserSessionsAsync(User user) =>
        await _db.UserSession.Where(r => r.User == user).ToListAsync();

    /*
     *
     ****/
    public async Task<User?> GetUserSessionByTokenAsync(string token) =>
        await _db
            .UserSession.Include(x => x.User)
            .Where(x => x.RefreshToken == token)
            .Select(x => x.User)
            .FirstOrDefaultAsync();

    /*
     *
     ****/
    public EntityEntry<UserSession> AddUserSession(UserSession session) =>
        _db.UserSession.Add(session);

    /*
     *
     ****/
    public EntityEntry<UserSession> RemoveUserSession(UserSession session) =>
        _db.UserSession.Remove(session);
}
