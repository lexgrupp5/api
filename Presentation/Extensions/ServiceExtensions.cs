using Data;
using Infrastructure.Persistence.Repositories;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Interfaces;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using System.Runtime.CompilerServices;

namespace Presentation.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
        }

        public static void ConfigureOpenApi(this IServiceCollection services) => services.AddEndpointsApiExplorer().AddSwaggerGen();

        public static void ConfigureServices(this IServiceCollection services)
        {
            //SERVICES GO HERE
            services.AddScoped<UserManager<User>, UserManager<User>>();
            services.AddScoped<IServiceCoordinator, ServiceCoordinator>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IModuleService, ModuleService>();

            services.AddScoped(provider => new Lazy<ICourseService>(() => provider.GetRequiredService<ICourseService>()));
            services.AddScoped(provider => new Lazy<IModuleService>(() => provider.GetRequiredService<IModuleService>()));
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            //REPOSITORIES GO HERE
            services.AddScoped<IDataCoordinator, DataCoordinator>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();

            services.AddScoped(provider => new Lazy<IActivityRepository>(() => provider.GetRequiredService<IActivityRepository>()));
            services.AddScoped(provider => new Lazy<IUserRepository>(() => provider.GetRequiredService<IUserRepository>()));
            services.AddScoped(provider => new Lazy<IModuleRepository>(() => provider.GetRequiredService<IModuleRepository>()));
            services.AddScoped(provider => new Lazy<IDocumentRepository>(() => provider.GetService<IDocumentRepository>()));
            services.AddScoped(provider => new Lazy<ICourseRepository>(() => provider.GetService<ICourseRepository>()));
        }
    }
}
