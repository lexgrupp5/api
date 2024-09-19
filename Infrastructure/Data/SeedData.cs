using Bogus;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data
{
    public static class SeedData
    {
        private static Faker faker = new Faker("sv");
        private static UserManager<User> _userManager = null!;
        private static RoleManager<IdentityRole> _roleManager = null!;
        private const string studentRole = "STUDENT";
        private const string teacherRole = "TEACHER";

        public static async Task InitializeAsync(AppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await context.Courses.AnyAsync())
            {
                Console.WriteLine("Existing data found. Aborting database seeding.");
                return;
            }

            _userManager = userManager;
            _roleManager = roleManager;

            var activityTypes = GenerateActivityTypes();
            await context.AddRangeAsync(activityTypes);
            await context.SaveChangesAsync();

            var courses = GenerateCourses(5, activityTypes);
            await context.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            try
            {
                await CreateRolesAsync([studentRole, teacherRole]);
                await context.SaveChangesAsync();

                await GenerateUsersAsync(50, courses);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        private static async Task CreateRolesAsync(string[] roleNames)
        {
            foreach (var roleName in roleNames)
            {
                if (await _roleManager.RoleExistsAsync(roleName)) continue;
                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }

        private static async Task GenerateUsersAsync(int numberOfUsers, IEnumerable<Course> courses)
        {
            string[] roles = ["STUDENT", "TEACHER"];
            var users = new Faker<User>("sv").Rules((faker, user) =>
            {
                user.Name = faker.Name.FullName();
                user.Email = user.Name.Replace(" ", "") + "@email.se";
                user.UserName = faker.Internet.UserName();
                user.Course = faker.PickRandom(courses);
            });

            var newUsers = users.Generate(numberOfUsers);

            foreach (var user in newUsers)
            {
                var result = await _userManager.CreateAsync(user, "Qwerty1234");
                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));

                await _userManager.AddToRoleAsync(user, faker.PickRandom(roles));

            }

        }

        private static List<ActivityType> GenerateActivityTypes()
        {
            var activityTypes = new List<string> { "Seminar", "Assignment", "Group project", "Setup" };

            //Creates a Faker-instance of ActivityType and adds a Description value.
            var activityDescriptions = new Faker<ActivityType>("en").Rules((faker, activityType) =>
            {
                activityType.Description = faker.Lorem.Sentence();
            });

            //Creates a list of ActivityType-objects based on the list "activityTypes". 
            var types = activityTypes.Select(activityName =>
            {
                var activity = activityDescriptions.Generate();
                activity.Name = activityName;
                return activity;
            }).ToList();

            return types;
        }

        private static List<Activity> GenerateActivities(int ammountOfActivities, List<ActivityType> types, DateTime startingDate)
        {
            var actitivies = new List<Activity>();
            DateTime lastEndDate = startingDate;

            for (int i = 0; i < ammountOfActivities; i++)
            {
                var startDate = lastEndDate.AddDays(1);
                var endDate = startDate.AddDays(faker.Random.Int(1,10));

                var activity = new Activity
                {
                    Description = faker.Lorem.Sentence(),
                    ActivityType = faker.PickRandom(types),
                    StartDate = startDate,
                    EndDate = endDate,
                };

                actitivies.Add(activity);
                lastEndDate = endDate;
            }
            return actitivies;
        }

        private static List<Module> GenerateModules(int ammountOfModules, List<ActivityType> activityTypes)
        {
            var moduleNames = new List<string> { "Module A", "Module B", "Module C", "Module D"};
            DateTime lastDate = faker.Date.Future();

            var modules = new List<Module>();

            for (int i = 0; i < ammountOfModules; i++)
            {
                var activities = GenerateActivities(faker.Random.Int(2,5), activityTypes, lastDate);

                var module = new Module
                {
                    Name = faker.PickRandom(moduleNames),
                    Description = faker.Lorem.Sentences(faker.Random.Int(1, 3)),
                    Activities = activities,
                    StartDate = activities.Min(a => a.StartDate),
                    EndDate = activities.Max(a => a.EndDate)
                };

                modules.Add(module);
                lastDate = activities.Max(a => a.EndDate);
            }

            return modules;

        }

        private static IEnumerable<Course> GenerateCourses(int ammountOfCourses, List<ActivityType> activityTypes)
        {
            var subjects = new List<string> { "Programming", "Mathematics", "History", "Physics", "Biology", "Art", "Psychology", "Economics", "Java", ".NET Programming", "Frontend", "Fullstack" };
            var levels = new List<string> { "Introduction to", "Advanced", "Fundamentals of", "Principles of", "Applied" };
            var suffixes = new List<string> { "101", "for Beginners", "in Practice", "and Society", "Theory" };

            var courses = new List<Course>();

            for (int i = 0; i < ammountOfCourses; i++)
            {
                var modules = GenerateModules(faker.Random.Int(2,6), activityTypes);

                var subject = faker.PickRandom(subjects);
                var level = faker.PickRandom(levels);
                var suffix = faker.PickRandom(suffixes);
                var courseName = $"{level} {subject} {suffix}";

                var course = new Course
                {
                    Name = courseName,
                    Description = faker.Lorem.Sentences(faker.Random.Int(1,3)),
                    Modules = modules,
                    StartDate = modules.Min(m => m.StartDate),
                    EndDate = modules.Max(m => m.EndDate)
                };

                courses.Add(course);
            }
            return courses;
        }
    }
}
