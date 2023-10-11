using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Companies;
using WebApplication1.Entities;
using Wolverine;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IMessageBus _bus;

    public CompanyController(ApplicationDbContext context, IMessageBus bus)
    {
        _context = context;
        _bus = bus;
    }
    
    [HttpGet]
    public async Task<IEnumerable<Company>> Index()
    {
        return await _context.Companies.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> PostCompany(Company company)
    {
        if (company.Id == Guid.Empty)
        {
            company.Id = Guid.NewGuid();
        }

        var command = new UpsertCompanyCommand() { Id = company.Id, Name = company.Name };
        await _bus.InvokeAsync(command);
        
        return Ok(company.Id);
    }
    
}