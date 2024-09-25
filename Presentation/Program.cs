using Application.Mapper;
using Presentation;
using Presentation.Extensions;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Context
builder.Services.ConfigureSql(builder.Configuration);

builder
    .Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddApplicationPart(typeof(AssemblyRef).Assembly);
builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddSwaggerGen(c =>
{
    // Add JWT authentication option to Swagger
    c.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Please insert JWT with Bearer into field. Example: Bearer {your_token}",
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        }
    );

    c.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference
                    {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        }
    );
});

// Identity
builder.Services.AddDataProtection();
builder.Services.ConfigureIdenttity();

builder.Services.ConfigureOpenApi();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.LoadOptions(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ApiExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDevCORS();
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.SeedDataAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
