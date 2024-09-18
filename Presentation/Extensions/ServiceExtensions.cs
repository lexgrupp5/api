using Data;
using Infrastructure.Persistence.Repositories;
using Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Extensions
{
    public static class ServiceExtensions
    {
        //public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
        //{
        //    services.AddDbContext<AppDbContext>(options => 
        //        options.UseSqlServer(configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
        //}
        public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("AppDbContext") ?? throw new InvalidOperationException("Connection string 'AppDbContext' not found.")));
        }

        public static void ConfigureOpenApi(this IServiceCollection services) => services.AddEndpointsApiExplorer().AddSwaggerGen();

        public static void ConfigureServices(this IServiceCollection services)
        {
            //SERVICES GO HERE
            services.AddScoped<IDataCoordinator, DataCoordinator>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            //REPOSITORIES GO HERE
        }

        public static void CreateRoles(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roles = new[] {"Student", "Teacher", "Admin"};

            foreach (var role in roles)
            {
                if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter();
                }
            }
        }
    }
}
