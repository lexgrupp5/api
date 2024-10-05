    using Application.Mapper;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using Presentation;
using Presentation.Extensions;
using Presentation.Filters;
using Presentation.Middlewares;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Context
        builder.Services.ConfigureSql(builder.Configuration);
        builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

        builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        builder
            .Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                /* options.Filters.Add<ValidateInputAttribute>(); */
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Formatting = Formatting.Indented;
            })
            .AddApplicationPart(typeof(AssemblyRef).Assembly);
        builder.Services.AddAutoMapper(typeof(MapperProfile));

// Services and Repositories
        builder.Services.ConfigureServices();
        builder.Services.ConfigureRepositories();

// Identity
        builder.Services.AddDataProtection();
        builder.Services.ConfigureIdenttity(builder.Configuration);

// OpenAPI
        builder.Services.ConfigureOpenApi();

// CORS
        builder.AddCORS(builder.Configuration);

        var app = builder.Build();

        app.UseMiddleware<JsonValidMiddleware>();
        app.UseMiddleware<DefaultHeaderMiddleware>();
        app.UseMiddleware<ApiExceptionMiddleware>();

// Configure the HTTP request pipeline.
        
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? builder.Environment.EnvironmentName;
        if (environmentName.Equals("Development"))
        {
            app.UseDevCORS();
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.SeedDataAsync();
        }
        else if (environmentName.Equals("Testing"))
        {
            app.UseTestCORS();
            app.UseSwagger();
            app.UseSwaggerUI();
            await app.SeedDataAsync();
        }
        else
        {
            //Default configuration for other environments
            app.UseAuthentication();
            app.UseAuthorization();
        }

        app.MapControllers();

        await app.RunAsync();
    }
}