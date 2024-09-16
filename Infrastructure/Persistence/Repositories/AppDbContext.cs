using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    DbSet<User> Users { get; set; }

    DbSet<Role> Roles { get; set; }

    DbSet<Course> Courses { get; set; }

    DbSet<Module> Modules { get; set; }

    DbSet<Document> Documents { get; set; }

    DbSet<Activity> Activities { get; set; }
}
