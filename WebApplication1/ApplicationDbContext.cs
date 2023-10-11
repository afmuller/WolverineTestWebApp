using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1;

public class ApplicationDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}