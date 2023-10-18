using SimpleWebApp.Core.Data.Repositories;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Data.Repositories;

public interface IEmployeeRepository : IRepository<Guid, Employee>
{
    Task<Employee[]> GetEmployeesAsync(IEnumerable<Guid> employeesIds, CancellationToken cancellationToken = default);
    Task<bool> IsEmployeeExistsAsync(string email, CancellationToken cancellationToken = default);
}