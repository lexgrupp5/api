﻿namespace Presentation.Extensions;

public static class CORSExtension
{
    private static readonly string _dev = "dev";

    private static readonly string _prod = "prod";
    
    private static readonly string _test = "Testing";

    public static void AddCORS(
        this WebApplicationBuilder builder,
        ConfigurationManager config)
    {
        var origins = config.GetSection("CORS:AllowedOrigins").Get<string[]>()
            ?? throw new InvalidOperationException("CORS origins 'CORS:AllowedOrigins' not found."
        );
        builder.Services.AddCors(options =>
            options.AddPolicy(_dev, builder => builder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            )
        );

        builder.Services.AddCors(options =>
            options.AddPolicy(_prod, builder => builder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            )
        );     
        builder.Services.AddCors(options =>
            options.AddPolicy(_test, builder => builder
                .WithOrigins(origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
            )
        );
    }

    public static void UseDevCORS(this WebApplication application)
    {
        application.UseCors(_dev);
    }

    public static void UseProdCORS(this WebApplication application)
    {
        application.UseCors(_prod);
    }   
    public static void UseTestCORS(this WebApplication application)
    {
        application.UseCors(_test);
    }
}
