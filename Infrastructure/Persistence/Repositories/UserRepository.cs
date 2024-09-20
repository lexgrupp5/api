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

    public async Task <IEnumerable<User>?> GetUsersFromCourseByIdAsync(int courseId)
    {
        var course = await _context.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync();
        if (course == null) { return null; }
        var students = await _context.Users.Where(u => u.CourseId == courseId).ToListAsync();
        return students;
        
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        //var user = await GetByConditionAsync(u => u.Name.Replace(" ", "").ToUpper() ==  username.ToUpper()).FirstAsync();
        var user = await _context.Users.Where(u => u.Name.Replace(" ","").ToUpper() == username.ToUpper()).FirstOrDefaultAsync();
        Console.WriteLine("Taking up space");
        if (user == null) { return null; }
        return user;
    }

    public async Task<bool> CheckUsernameExistsAsync(User user)
    {
        return await _context.Users.AnyAsync(u => u.Name == user.Name);
    }
}