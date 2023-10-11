using WebApplication1.Entities;

namespace WebApplication1;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        if (!context.Companies.Any())
        {
            context.Companies.Add(new Company
            {
                Id = Guid.NewGuid(),
                Name = "ABC Trading",
            });
            context.Companies.Add(new Company
            {
                Id = Guid.NewGuid(),
                Name = "XYZ Manufacturing",
            });
            
            await context.SaveChangesAsync();
        }
    }
}