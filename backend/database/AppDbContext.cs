using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Database;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Users> Users => Set<Users>();
    public DbSet<Categories> Categories => Set<Categories>();
    public DbSet<Products> Products => Set<Products>();
}