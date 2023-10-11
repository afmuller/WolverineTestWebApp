using Microsoft.EntityFrameworkCore;
using Oakton;
using Oakton.Resources;
using WebApplication1;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.FluentValidation;
using Wolverine.Postgresql;
using Wolverine.SqlServer;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextWithWolverineIntegration<ApplicationDbContext>(
    options => options.UseSqlServer(
        connectionString, b =>
        {
            b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        }
    )
);

builder.Services.AddResourceSetupOnStartup();

builder.Host.UseWolverine(opts =>
{
    opts.UseFluentValidation();
    opts.PersistMessagesWithSqlServer(connectionString);
    opts.UseEntityFrameworkCoreTransactions();
    opts.Policies.UseDurableLocalQueues();
    opts.Policies.AutoApplyTransactions();
    opts.OptimizeArtifactWorkflow();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (context.Database.IsNpgsql() || context.Database.IsSqlServer())
        {
            context.Database.Migrate();
        }

        await ApplicationDbContextSeed.SeedSampleDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunOaktonCommands(args);
