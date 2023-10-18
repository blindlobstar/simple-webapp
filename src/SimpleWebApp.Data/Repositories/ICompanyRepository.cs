using SimpleWebApp.Data.Core;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Data.Repositories;

public interface ICompanyRepository : IRepository<Guid, Company>
{
    Task<Company[]> GetCompaniesWithEmployeesAsync(Guid[] companiesIds, CancellationToken cancellationToken = default);
    Task<bool> IsCompanyExistsAsync(string name, CancellationToken cancellationToken = default);
}