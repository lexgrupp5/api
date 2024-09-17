using Data;
using Infrastructure.Persistence.Repositories;
using Service;
using Microsoft.EntityFrameworkCore;

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
    }
}
