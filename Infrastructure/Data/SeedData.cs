using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Domain.Entities;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Infrastructure.Data
{
    public class SeedData
    {
        private static Faker faker = new Faker("sv");

        public static async Task InitializeAsync(AppDbContext context)
        {
            //if (await context.Courses.AnyAsync())
            //{
            //    Console.WriteLine("Existing data found. Aborting database seeding.");
            //    return;
            //}

            var activityTypes = GenerateActivityTypes();
            await context.AddRangeAsync(activityTypes);
            await context.SaveChangesAsync();

            var courses = GenerateCourses(5, activityTypes);
            await context.AddRangeAsync(courses);
            await context.SaveChangesAsync();

            var students = GenerateStudents(5);
            await context.AddRangeAsync(students);
            await context.SaveChangesAsync();
        }
        //generates students with names and courseId, rest is NULL.
        private static List<User> GenerateStudents(int amountOfStudents)
        {
            var students = new List<User>();
            var role = new Role { Name = "Student" };

            var faker = new Faker("sv");
            for (int i = 0; i < amountOfStudents; i++)
            {
                var userName = faker.Person.FullName;
                var courseId = faker.Random.Int(1, 5);

                var user = new User { Name = userName, Role = role, CourseId = courseId };
                students.Add(user);
            }

            return students;
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
