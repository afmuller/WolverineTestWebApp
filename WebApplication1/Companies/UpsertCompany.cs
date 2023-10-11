using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;
using Wolverine;

namespace WebApplication1.Companies;

public class UpsertCompanyCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class UpsertCompanyCommandValidator : AbstractValidator<UpsertCompanyCommand>
{
    private readonly ApplicationDbContext _context;

    public UpsertCompanyCommandValidator(ApplicationDbContext context)
    {
        _context = context;
        RuleFor(v => v.Id).NotEmpty();
        RuleFor(v => v.Name).NotEmpty();
        RuleFor(v => v.Name).MustAsync(NameMustNotExist).WithMessage("A company with this name already exists");
    }

    async Task<bool> NameMustNotExist(UpsertCompanyCommand command, string name, CancellationToken cancellationToken)
    {
        return await _context.Companies.CountAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != command.Id, cancellationToken) == 0 ? true : false;
    }
}

public static class UpsertCompanyCommandHandler
{
    public static async Task<OutgoingMessages> Handle(UpsertCompanyCommand command, ApplicationDbContext context)
    {
        var messages = new OutgoingMessages();
        var model = await context.Companies.FirstOrDefaultAsync(p => p.Id == command.Id);
        if (model == null)
        {
            model = new Company() { Id = command.Id, Name = command.Name };
            context.Companies.Add(model);
            messages.Add(new CompanyCreated(model.Id, model.Name));
        }
        else
        {
            model.Name = command.Name;
            messages.Add(new CompanyUpdated(model.Id, model.Name));
        }

        return messages;
    }
}

public record CompanyCreated(Guid Id, string Name);
public record CompanyUpdated(Guid Id, string Name);