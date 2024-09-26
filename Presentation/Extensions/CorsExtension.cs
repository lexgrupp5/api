namespace Presentation.Extensions;

public static class CORSExtension
{
    private static readonly string _dev = "dev";

    private static readonly string _prod = "prod";

    public static void AddCORS(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
            options.AddPolicy(
                _dev,
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            )
        );

        builder.Services.AddCors(options =>
            options.AddPolicy(
                _prod,
                builder => builder.WithOrigins("").AllowAnyMethod().AllowAnyHeader()
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
}
