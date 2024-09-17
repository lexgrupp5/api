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

        public DbSet<ActivityType> ActivityType => Set<ActivityType>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);

    //    // Seed för ActivityType
    //    modelBuilder.Entity<ActivityType>().HasData(
    //        new ActivityType { Id = 1, Name = "Seminar", Description = "Seminar type activity" },
    //        new ActivityType { Id = 2, Name = "Assignment", Description = "Assignment type activity" },
    //        new ActivityType { Id = 3, Name = "Group project", Description = "Group project activity" },
    //        new ActivityType { Id = 4, Name = "Setup", Description = "Setup type activity" }
    //    );

    //    // Seed för Activity
    //    modelBuilder.Entity<Activity>().HasData(
    //        new Activity { Id = 1, Description = "First activity", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1), ActivityTypeId = 1, ModuleId = 1 },
    //        new Activity { Id = 2, Description = "Second activity", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(2), ActivityTypeId = 2, ModuleId = 1 }
    //    // Lägg till fler om du behöver
    //    );
    //}

}

