using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace Presentation.Extensions;

public static class SeedExtensions
{
    public static async Task SeedDataAsync(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        //await context.Database.EnsureDeletedAsync();
        //await context.Database.MigrateAsync();

        try
        {
            await SeedData.InitializeAsync(context, userManager, roleManager);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}
