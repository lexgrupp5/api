using Application.Mapper;
using Presentation;
using Presentation.Extensions;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Context
builder.Services.ConfigureSql(builder.Configuration);
builder.Services.AddRouting(opt => opt.LowercaseUrls = true);

builder
    .Services.AddControllers(configure => configure.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddApplicationPart(typeof(AssemblyRef).Assembly);
builder.Services.AddAutoMapper(typeof(MapperProfile));

// Services and Repositories
builder.Services.ConfigureServices();
builder.Services.ConfigureRepositories();

// Identity
builder.Services.AddDataProtection();
builder.Services.ConfigureIdenttity(builder.Configuration);

builder.Services.ConfigureOpenApi();
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
