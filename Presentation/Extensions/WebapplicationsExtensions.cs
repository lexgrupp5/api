using System.Runtime.CompilerServices;

using Domain.Entities;

using Infrastructure.Data;
using Infrastructure.Persistence.Repositories;

using Microsoft.AspNetCore.Identity;

namespace Presentation.Extensions
{
    public static class WebapplicationsExtensions
    {

        public static async Task SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<AppDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                try
                {
                    await SeedData.InitializeAsync(context, userManager, roleManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
