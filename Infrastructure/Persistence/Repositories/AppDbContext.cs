using Domain.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Repositories;

public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public AppDbContext (DbContextOptions<AppDbContext> options) 
        : base(options){}

    //public DbSet<User> Users => Set<User>();

    //public DbSet<Role> Roles => Set<Role>();

    public DbSet<Course> Courses => Set<Course>();

        public DbSet<Module> Modules => Set<Module>();

        public DbSet<Document> Documents => Set<Document>();

        public DbSet<Activity> Activities => Set<Activity>();

        public DbSet<ActivityType> ActivityType => Set<ActivityType>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasDiscriminator<string>("Discriminator")
            .HasValue<User>("User");
    }
}

