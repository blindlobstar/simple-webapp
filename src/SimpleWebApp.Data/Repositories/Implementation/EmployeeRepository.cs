using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleWebApp.Data.SysLog.Mappers;
using SimpleWebApp.Data.SysLog.Repositories;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Data.Repositories.Implementation;

public sealed class EmployeeRepository : LogRepository<Guid, Employee>, IEmployeeRepository
{
    public EmployeeRepository(
        SimpleDBContext dBContext,
        ILogger<EmployeeRepository> logger,
        ISysLogMapper<Guid, Employee> sysLogMapper) : base(dBContext, logger, sysLogMapper)
    {
    }

    public Task<Employee[]> GetEmployeesAsync(IEnumerable<Guid> employeeIds, CancellationToken cancellationToken = default) => 
        DbSet
            .Where(e => employeeIds.Contains(e.Id))
            .ToArrayAsync(cancellationToken);

    public Task<bool> IsEmployeeExistsAsync(string email, CancellationToken cancellationToken = default) =>
        DbSet.AnyAsync(x => x.Email == email, cancellationToken: cancellationToken);
}
