using System.Security.Claims;
using System.Text;
using Application.Coordinator;
using Application.Interfaces;
using Application.Services;

using Domain.Configuration;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Coordinators;
using Infrastructure.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                var jwtOptions = configuration.GetSection("Jw1tOptions");
                var authTokenOptions = jwtOptions.Get<AuthTokenOptions>();
                ArgumentNullException.ThrowIfNull(jwtOptions, nameof(jwtOptions));
                ArgumentNullException.ThrowIfNull(authTokenOptions, nameof(authTokenOptions));


                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JwtOptions:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JwtOptions:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["JwtOptions:Secret"]!)
                    ),
                };
            }
            );
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                "TeacherPolicy",
                policy =>
                    policy
                        .RequireRole(UserRoles.Teacher)
                        .RequireClaim(ClaimTypes.NameIdentifier)
                        .RequireClaim(ClaimTypes.Role)
            );

            options.AddPolicy(
                "StudentPolicy",
                policy =>
                    policy
                        .RequireRole(UserRoles.Student)
                        .RequireClaim(ClaimTypes.NameIdentifier)
                        .RequireClaim(ClaimTypes.Role)
            );
        });
    }

    public static void ConfigureIdenttity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureOpenApi(this IServiceCollection services) =>
        services.AddEndpointsApiExplorer().AddSwaggerGen();

    public static void LoadOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthTokenOptions>(configuration.GetSection("JwtOptions"));
        services.AddSingleton(service =>
            service.GetRequiredService<IOptions<AuthTokenOptions>>().Value
        );
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

        // Lazy
        services.AddScoped(provider => new Lazy<IActivityRepository>(
            () => provider.GetRequiredService<IActivityRepository>()
        ));
        services.AddScoped(provider => new Lazy<IUserRepository>(
            () => provider.GetRequiredService<IUserRepository>()
        ));
        services.AddScoped(provider => new Lazy<IModuleRepository>(
            () => provider.GetRequiredService<IModuleRepository>()
        ));
        services.AddScoped(provider => new Lazy<IDocumentRepository>(
            () => provider.GetRequiredService<IDocumentRepository>()
        ));
        services.AddScoped(provider => new Lazy<ICourseRepository>(
            () => provider.GetRequiredService<ICourseRepository>()
        ));
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        //SERVICES GO HERE
        services.AddScoped<IServiceCoordinator, ServiceCoordinator>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<UserManager<User>, UserManager<User>>();
        services.AddSingleton<ITokenService, TokenService>();

        // Lazy
        services.AddScoped(provider => new Lazy<ICourseService>(
            () => provider.GetRequiredService<ICourseService>()
        ));
        services.AddScoped(provider => new Lazy<IIdentityService>(
            () => provider.GetRequiredService<IIdentityService>()
        ));
        services.AddScoped(provider => new Lazy<IModuleService>(
            () => provider.GetRequiredService<IModuleService>()
        ));
        services.AddScoped(provider => new Lazy<IUserService>(
            () => provider.GetRequiredService<IUserService>()
        ));
    }

    public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AppDbContext")
                    ?? throw new InvalidOperationException(
                        "Connection string 'AppDbContext' not found."
                    )
            )
        );
    }
}
