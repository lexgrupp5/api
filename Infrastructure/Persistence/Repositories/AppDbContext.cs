using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Repositories;

public class AppDbContext : DbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options) 
        : base(options){}
    
        public DbSet<User> Users => Set<User> ();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Course> Courses => Set<Course>();

        public DbSet<Module> Modules => Set<Module>();

        public DbSet<Document> Documents => Set<Document>();

        public DbSet<Activity> Activities => Set<Activity>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

}

