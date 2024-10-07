using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<ActivityType> ActivityType => Set<ActivityType>();
    public DbSet<UserSession> UserSession => Set<UserSession>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<Course>()
            .HasOne(c => c.Teacher)
            .WithMany()
            .HasForeignKey(c => c.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var activities = ChangeTracker.Entries<Activity>().ToList();

        if (activities.Count > 0)
            await HandleActivity(activities);

        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task HandleActivity(List<EntityEntry<Activity>> activities)
    {
        if (activities.Count <= 0)
            return;

        foreach (var entry in activities)
        {
            var isValidState = entry.State switch
            {
                EntityState.Added => true,
                EntityState.Modified => true,
                EntityState.Deleted => true,
                _ => false
            };

            if (!isValidState)
                continue;

            var activity = entry.Entity;
            if (!Entry(activity).Reference(a => a.Module).IsLoaded)
                await Entry(activity).Reference(a => a.Module).LoadAsync();

            if (!Entry(activity.Module).Reference(m => m.Course).IsLoaded)
                await Entry(activity.Module).Reference(m => m.Course).LoadAsync();

            switch (entry.State)
            {
                case EntityState.Added:
                    HandleActivityAdded(activity);
                    break;
                case EntityState.Modified:
                    await HandleActivityModified(activity, entry);
                    break;
                case EntityState.Deleted:
                    await HandleActivityDeleted(activity);
                    break;
                default:
                    break;
            }
        }
    }

    private async Task HandleActivityDeleted(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        var module = activity.Module;
        var course = activity.Module.Course;

        if (!Entry(module).Collection(m => m.Activities).IsLoaded)
            await Entry(module).Collection(m => m.Activities).LoadAsync();

        if (!Entry(course).Collection(m => m.Modules).IsLoaded)
            await Entry(course).Collection(m => m.Modules).LoadAsync();

        var activities = module.Activities.ToList();

        activities.Remove(activity);

        if (module.Activities.Count == 0)
        {
            var oldModuleStartDate = module.StartDate;
            var oldModuleEndDate = module.EndDate;
            module.StartDate = null;
            module.EndDate = null;
            if (oldModuleStartDate == course.StartDate)
            {
                var newCourseStartDate = module.Activities.Min(a => a.StartDate);
                course.StartDate = newCourseStartDate == default ? null : newCourseStartDate;
            }

            if (oldModuleEndDate == course.EndDate)
            {
                var newCourseEndDate = module.Activities.Max(a => a.EndDate);
                course.EndDate = newCourseEndDate == default ? null : newCourseEndDate;
            }
        }

        if (activity.StartDate == module.StartDate)
        {
            var newModuleStartDate = module.Activities.Min(a => a.StartDate);
            if (module.StartDate == course.StartDate)
                course.StartDate = newModuleStartDate;
            module.StartDate = newModuleStartDate;
        }

        if (activity.EndDate == module.EndDate)
        {
            var newModuleEndDate = module.Activities.Max(a => a.EndDate);
            if (module.EndDate == course.EndDate)
                course.EndDate = newModuleEndDate;
            module.EndDate = newModuleEndDate;
        }
    }

    private async Task HandleActivityModified(Activity activity, EntityEntry<Activity> entry)
    {
        ArgumentNullException.ThrowIfNull(activity);

        var module = activity.Module;
        var course = activity.Module.Course;

        var oldStart = entry.Property(a => a.StartDate).OriginalValue;
        var oldEnd = entry.Property(a => a.EndDate).OriginalValue;

        var isStartNewer = activity.StartDate < module.StartDate;
        var isEndNewer = activity.EndDate < module.EndDate;

        var isFirstActivity = oldStart == module.StartDate;
        var isLastActivity = oldEnd == module.EndDate;

        if (!isFirstActivity && !isLastActivity)
            return;

        if (!Entry(module).Collection(m => m.Activities).IsLoaded)
            await Entry(module).Collection(m => m.Activities).LoadAsync();

        if (!Entry(course).Collection(m => m.Modules).IsLoaded)
            await Entry(course).Collection(m => m.Modules).LoadAsync();

        if (isFirstActivity)
        {
            if (module.StartDate == course.StartDate)
                course.StartDate = activity.StartDate;
            module.StartDate = activity.StartDate;
        }

        if (isLastActivity)
        {
            if (module.EndDate == course.EndDate)
                course.EndDate = activity.EndDate;
            module.EndDate = activity.EndDate;
        }
    }

    private static void HandleActivityAdded(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        var module = activity.Module;
        var course = activity.Module.Course;

        if (activity.StartDate < module.StartDate)
        {
            if (module.StartDate == course.StartDate)
                course.StartDate = activity.StartDate;
            module.StartDate = activity.StartDate;
        }

        if (activity.EndDate > module.EndDate)
        {
            if (module.EndDate == course.EndDate)
                course.EndDate = activity.EndDate;
            module.EndDate = activity.EndDate;
        }
    }
}
