using JasperFx.Core;

namespace WebApplication1.Companies;

public static class CompanyCreatedHandler
{
    public static async Task Handle(CompanyCreated command)
    {
        Console.WriteLine($"Company created with ID {command.Id} NAME {command.Name}");
    }
}

public static class CompanyUpdatedHandler
{
    public static async Task Handle(CompanyUpdated command)
    {
        await Task.Delay(20.Seconds());
        Console.WriteLine($"Company updated with ID {command.Id} NAME {command.Name}");
    }
}