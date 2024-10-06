using Microsoft.AspNetCore.Cors.Infrastructure;

using Presentation.Constants;

namespace Presentation.Extensions;

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
        
        void AddCorsPolicy(string policyName, bool allowCredentials)
        {
            builder.Services.AddCors(options =>
                options.AddPolicy(policyName, builder => builder
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders(CustomHeader.Pagination)
                    .SetCredentials(allowCredentials)
                )
            );
        }

        AddCorsPolicy(_dev, true);
        AddCorsPolicy(_prod, true);
        AddCorsPolicy(_test, false);
    }

    private static CorsPolicyBuilder SetCredentials(this CorsPolicyBuilder builder, bool allowCredentials)
    {
        return allowCredentials ? builder.AllowCredentials() : builder;
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
