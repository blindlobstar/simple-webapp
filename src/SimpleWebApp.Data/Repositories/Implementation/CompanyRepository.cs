using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleWebApp.Data.SysLog.Mappers;
using SimpleWebApp.Data.SysLog.Repositories;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Data.Repositories.Implementation;

public sealed class CompanyRepository : LogRepository<Guid, Company>, ICompanyRepository
{
    public CompanyRepository(
        SimpleDBContext dBContext,
        ILogger<CompanyRepository> logger,
        ISysLogMapper<Guid, Company> sysLogMapper) : base(dBContext, logger, sysLogMapper)
    {
    }

    public async Task<Company[]> GetCompaniesWithEmployeesAsync(Guid[] companiesIds, CancellationToken cancellationToken = default)
    {
        var companiesWithEmployees = await DbSet 
            .Where(company => companiesIds.Contains(company.Id))
            .Include(company => company.Employees)
            .ToArrayAsync(cancellationToken);

        return companiesWithEmployees;
    }

    public Task<bool> IsCompanyExistsAsync(string name, CancellationToken cancellationToken = default) => 
       DbSet.AnyAsync(x => x.Name == name, cancellationToken: cancellationToken);
}
