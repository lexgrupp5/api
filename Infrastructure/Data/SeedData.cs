using Bogus;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Models;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SeedData
{
    private static readonly Faker Faker = new("sv");
    private static UserManager<User> s_userManager = null!;
    private static RoleManager<IdentityRole> s_roleManager = null!;
    private const string StudentRole = UserRoles.Student;
    private const string TeacherRole = UserRoles.Teacher;

    public static async Task InitializeAsync(
        AppDbContext context,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        if (await context.Courses.AnyAsync())
        {
            Console.WriteLine("Existing data found. Aborting database seeding.");
            return;
        }

        s_userManager = userManager;
        s_roleManager = roleManager;

        var activityTypes = GenerateActivityTypes();
        await context.AddRangeAsync(activityTypes);
        await context.SaveChangesAsync();

        var courses = GenerateCourses(40, activityTypes);
        await context.AddRangeAsync(courses);
        await context.SaveChangesAsync();

        try
        {
            await GenerateRolesAsync([StudentRole, TeacherRole]);
            await context.SaveChangesAsync();

            await GenerateUsersAsync(500, courses);
            await context.SaveChangesAsync();

            var testUsers = GenerateTestUsers(courses);
            await AddFakeUsers(testUsers, userManager);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    private static async Task GenerateRolesAsync(string[] roleNames)
    {
        foreach (var roleName in roleNames)
        {
            if (await s_roleManager.RoleExistsAsync(roleName))
                continue;
            var role = new IdentityRole(roleName);
            var result = await s_roleManager.CreateAsync(role);

            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));
        }
    }

    private static async Task<bool> AddFakeUsers(
        List<FakeUser> fakeUsers,
        UserManager<User> userManager
    )
    {
        foreach (var fake in fakeUsers)
        {
            try
            {
                var user = new User()
                {
                    UserName = fake.UserName,
                    Name = fake.Name,
                    Email = fake.Email,
                    Course = fake.Course
                };
                await userManager.CreateAsync(user, "Qwerty1234");
                await userManager.AddToRoleAsync(user, fake.RoleName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        return true;
    }

    private static List<FakeUser> GenerateTestUsers(IEnumerable<Course> courses)
    {
        var testStudent = new FakeUser()
        {
            UserName = "test-student",
            Name = "Test Student",
            Email = "test-student@email.se",
            Course = Faker.PickRandom(Faker.Random.Shuffle(courses)),
            RoleName = StudentRole
        };
        var testTeacher = new FakeUser()
        {
            UserName = "test-teacher",
            Name = "Test Teacher",
            Email = "test-teacher@email.se",
            Course = Faker.PickRandom(Faker.Random.Shuffle(courses)),
            RoleName = TeacherRole
        };
        return [testStudent, testTeacher];
    }

    private static async Task GenerateUsersAsync(int numberOfUsers, IEnumerable<Course> courses)
    {
        int courseId = 1;
        var numberOfCourses = courses.Count();

        var generateTeachers = new Faker<User>("sv").Rules(
            (faker, teacher) =>
            {
                teacher.Name = faker.Name.FullName();
                teacher.Email = teacher.Name.Replace(" ", "") + "@email.se";
                teacher.UserName = faker.Internet.UserName();
                teacher.Course = courses.Where(c => c.Id == courseId).FirstOrDefault();
                courseId++;
            }
        );

        var teachers = generateTeachers.Generate(numberOfCourses);

        var generateStudents = new Faker<User>("sv").Rules(
            (faker, student) =>
            {
                student.Name = faker.Name.FullName();
                student.Email = student.Name.Replace(" ", "") + "@email.se";
                student.UserName = faker.Internet.UserName();
                student.Course = faker.PickRandom(courses);
            }
        );

        var students = generateStudents.Generate(numberOfUsers - numberOfCourses);

        for (int i = 0; i < numberOfCourses; i++)
        {
            var result = await s_userManager.CreateAsync(teachers[i], "Qwerty1234");
            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));
            await s_userManager.AddToRoleAsync(teachers[i], TeacherRole);
        }

        for (int i = 0; i < numberOfUsers - numberOfCourses; i++)
        {
            var result = await s_userManager.CreateAsync(students[i], "Qwerty1234");
            if (!result.Succeeded)
                throw new Exception(string.Join("\n", result.Errors));

            await s_userManager.AddToRoleAsync(students[i], StudentRole);
        }
    }

    private static List<ActivityType> GenerateActivityTypes()
    {
        var activityTypes = new List<string> { "Seminar", "Assignment", "Group project", "Setup" };

        //Creates a Faker-instance of ActivityType and adds a Description value.
        var activityDescriptions = new Faker<ActivityType>("en").Rules(
            (faker, activityType) =>
            {
                activityType.Description = faker.Lorem.Sentence();
            }
        );

        //Creates a list of ActivityType-objects based on the list "activityTypes".
        var types = activityTypes
            .Select(activityName =>
            {
                var activity = activityDescriptions.Generate();
                activity.Name = activityName;
                return activity;
            })
            .ToList();

        return types;
    }

    private static List<Activity> GenerateActivities(
        int ammountOfActivities,
        List<ActivityType> types,
        DateTime startingDate
    )
    {
        var actitivies = new List<Activity>();
        DateTime lastEndDate = startingDate;

        for (int i = 0; i < ammountOfActivities; i++)
        {
            var startDate = lastEndDate.AddDays(1);
            var endDate = startDate.AddDays(Faker.Random.Int(1, 10));

            var activity = new Activity
            {
                Description = Faker.Lorem.Sentence(),
                ActivityType = Faker.PickRandom(types),
                StartDate = startDate,
                EndDate = endDate,
            };

            actitivies.Add(activity);
            lastEndDate = endDate;
        }
        return actitivies;
    }

    private static List<Module> GenerateModules(
        int ammountOfModules,
        List<ActivityType> activityTypes
    )
    {
        var moduleNames = new List<string> { "Module A", "Module B", "Module C", "Module D" };
        DateTime lastDate = Faker.Date.Future();

        var modules = new List<Module>();

        for (int i = 0; i < ammountOfModules; i++)
        {
            var activities = GenerateActivities(Faker.Random.Int(2, 5), activityTypes, lastDate);

            var module = new Module
            {
                Name = Faker.PickRandom(moduleNames),
                Description = Faker.Lorem.Sentences(Faker.Random.Int(1, 3)),
                Activities = activities,
                StartDate = activities.Min(a => a.StartDate),
                EndDate = activities.Max(a => a.EndDate)
            };

            modules.Add(module);
            lastDate = activities.Max(a => a.EndDate);
        }

        return modules;
    }

    private static IEnumerable<Course> GenerateCourses(
        int ammountOfCourses,
        List<ActivityType> activityTypes
    )
    {
        var subjects = new List<string>
        {
            "Programming",
            "Mathematics",
            "History",
            "Physics",
            "Biology",
            "Art",
            "Psychology",
            "Economics",
            "Java",
            ".NET Programming",
            "Frontend",
            "Fullstack"
        };
        var levels = new List<string>
        {
            "Introduction to",
            "Advanced",
            "Fundamentals of",
            "Principles of",
            "Applied"
        };
        var suffixes = new List<string>
        {
            "101",
            "for Beginners",
            "in Practice",
            "and Society",
            "Theory"
        };

        var courses = new List<Course>();

        for (int i = 0; i < ammountOfCourses; i++)
        {
            var modules = GenerateModules(Faker.Random.Int(2, 6), activityTypes);

            var subject = Faker.PickRandom(subjects);
            var level = Faker.PickRandom(levels);
            var suffix = Faker.PickRandom(suffixes);
            var courseName = $"{level} {subject} {suffix}";

            var course = new Course
            {
                Name = courseName,
                Description = Faker.Lorem.Sentences(Faker.Random.Int(1, 3)),
                Modules = modules,
                StartDate = modules.Min(m => m.StartDate),
                EndDate = modules.Max(m => m.EndDate)
            };

            courses.Add(course);
        }
        return courses;
    }
}
