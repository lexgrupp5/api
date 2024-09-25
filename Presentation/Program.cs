using System.Security.Claims;
using System.Text;
using Application.Mapper;
using Domain.Configuration;
using Domain.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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



// Identity
/* builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization(); */
builder.Services.AddDataProtection();
builder.Services.ConfigureIdenttity();

builder
    .Services.AddAuthentication(o =>
    {
        o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var jwtOptions = builder.Configuration.GetSection("JwtOptions");
        var authTokenOptions = jwtOptions.Get<AuthTokenOptions>();
        ArgumentNullException.ThrowIfNull(jwtOptions, nameof(jwtOptions));
        ArgumentNullException.ThrowIfNull(authTokenOptions, nameof(authTokenOptions));

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authTokenOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authTokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(authTokenOptions.Secret)
            ),
        };
    });

builder
    .Services.AddAuthorizationBuilder()
    .AddPolicy(
        "TeacherPolicy",
        policy =>
            policy
                .RequireRole(UserRoles.Teacher)
                .RequireClaim(ClaimTypes.NameIdentifier)
                .RequireClaim(ClaimTypes.Role)
    )
    .AddPolicy(
        "StudentPolicy",
        policy =>
            policy
                .RequireRole(UserRoles.Student)
                .RequireClaim(ClaimTypes.NameIdentifier)
                .RequireClaim(ClaimTypes.Role)
    );

builder.Services.ConfigureOpenApi();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();
builder.Services.LoadOptions(builder.Configuration);

builder
    .Services.AddEndpointsApiExplorer()
    .AddSwaggerGen(setup =>
    {
        setup.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Place to add JWT with Bearer",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            }
        );

        setup.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            }
        );
    });

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
