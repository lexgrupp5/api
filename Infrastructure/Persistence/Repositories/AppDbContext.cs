using Domain.Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AppDbContext : IdentityDbContext
{
    public AppDbContext (DbContextOptions<AppDbContext> options) 
        : base(options){}
    
        public DbSet<User> Users => Set<User> ();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Document> Documents => Set<Document>();
        public DbSet<Activity> Activities => Set<Activity>();
        public DbSet<ActivityType> ActivityType => Set<ActivityType>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
    }
}

