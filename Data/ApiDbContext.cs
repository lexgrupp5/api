using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options) : DbContext(options)
{
    DbSet<Example> Examples { get; set; }
}