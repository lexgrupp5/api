using Domain.Entities;

using Infrastructure.Persistence;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TestBackend;
//TODO Other database
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public AppDbContext Context { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {
            var dbContextOptions =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextOptions != null)
                services.Remove(dbContextOptions);

            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });
            
            var scope = services.BuildServiceProvider().CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // // Populate in-memory database with sample data
            // context.Users.AddRange(
            //     new User { Name = "Tom", Email = "tom@mail.com", UserName = "Test" }
            // );
            context.SaveChanges();
            
            Context = context;
        });
    }
}