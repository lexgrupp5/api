using System.Reflection.Metadata;
using Application;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Presentation;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(configuration.GetConnectionString("MovieCardContext") ?? throw new InvalidOperationException("Connection string 'MovieCardContext' not found.")));
builder.Services.ConfigureSql(builder.Configuration);
builder.Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
                //.AddNewtonsoftJson()
                .AddApplicationPart(typeof(AssemblyRef).Assembly);

builder.Services.AddAutoMapper(typeof(MapperProfile));
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

app.UseAuthorization();

app.MapControllers();

app.Run();
