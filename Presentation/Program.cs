using Application.Mapper;
using Infrastructure.Persistence.Repositories;
using Presentation;
using Presentation.Extensions;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.ConfigureSql(builder.Configuration);

builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
    //.AddNewtonsoftJson()
    .AddApplicationPart(typeof(AssemblyRef).Assembly);

builder.Services.AddAutoMapper(typeof(MapperProfile));


//builder.Services.AddTransient(s => new Lazy<UserManager<User>>(() => s.GetRequiredService<UserManager<User>>()));

// Identity
builder.Services.AddDataProtection();
builder.Services.AddIdentityCore<User>(options =>
{
    // Password settings
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

builder.Services.ConfigureOpenApi();
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.SeedDataAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();