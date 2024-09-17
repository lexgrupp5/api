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
            //    Console.WriteLine("Existing data found. Ending seeding.");
            //    return;
            //}

            var activityTypes = GenerateActivityTypes();
            await context.AddRangeAsync(activityTypes);
            await context.SaveChangesAsync();

            var activities = GenerateActivities(5, activityTypes);
            await context.AddRangeAsync(activities);
            await context.SaveChangesAsync();

            var modules = GenerateModules(6, activities);
            await context.AddRangeAsync(modules);
            await context.SaveChangesAsync();

            var courses = GenerateCourses(10, modules);
            await context.AddRangeAsync(courses);

            await context.SaveChangesAsync();
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

        private static List<Activity> GenerateActivities(int v, List<ActivityType> activityTypes)
        {
            var startingDate = faker.Date.Future();

            var activities = new Faker<Activity>("en").Rules((faker, activity) =>
            {
                activity.Description = faker.Lorem.Sentences(faker.Random.Int(1, 3));
                activity.ActivityTypeId = faker.PickRandom(activityTypes).Id;
                activity.StartDate = startingDate.AddDays(1);
                activity.EndDate = faker.Date.Between(activity.StartDate, activity.StartDate.AddDays(faker.Random.Int(1, 10)));
                startingDate = activity.EndDate;
            });

            return activities.Generate(v);
        }

        private static List<Module> GenerateModules(int v, List<Activity> activities)
        {
            //Generate activities to be added to the module
            var moduleNames = new List<string> { "Beginner", "Intermediary", "Novice", "Roundup"};

            var modules = new Faker<Module>("en").Rules((faker, module) =>
            {
                module.Name = faker.PickRandom(moduleNames);
                module.Description = faker.Lorem.Sentences(faker.Random.Int(1, 3));
                module.Activities = activities;
                module.StartDate = activities.First().StartDate;
                module.EndDate = activities.Last().EndDate;
            });

            return modules.Generate(v);
        }

        private static IEnumerable<Course> GenerateCourses(int v, List<Module> modules)
        {
            var subjects = new List<string> { "Programming", "Mathematics", "History", "Physics", "Biology", "Art", "Psychology", "Economics", "Java", ".NET Programming", "Frontend", "Fullstack" };
            var levels = new List<string> { "Introduction to", "Advanced", "Fundamentals of", "Principles of", "Applied" };
            var suffixes = new List<string> { "101", "for Beginners", "in Practice", "and Society", "Theory" };

            var courses = new Faker<Course>("en").Rules((faker, course) =>
            {
                var subject = faker.PickRandom(subjects);
                var level = faker.PickRandom(levels);
                var suffix = faker.PickRandom(suffixes);
                var courseName = $"{level} {subject} {suffix}";
                course.Name = courseName;
                course.Description = faker.Lorem.Sentences(faker.Random.Int(1, 3));
                course.Modules = modules;
                course.StartDate = modules.First().StartDate;
                course.EndDate = modules.Last().EndDate;
            });

            return courses.Generate(v);
        }
    }
}
