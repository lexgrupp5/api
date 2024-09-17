using System.Runtime.CompilerServices;

using Infrastructure.Data;
using Infrastructure.Persistence.Repositories;

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

                try
                {
                    await SeedData.InitializeAsync(context);
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
