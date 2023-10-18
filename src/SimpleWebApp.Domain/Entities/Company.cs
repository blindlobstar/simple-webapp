using SimpleWebApp.Domain.Core.Entities;

namespace SimpleWebApp.Domain.Entities;

public class Company : EntityBase<Guid>
{
    public required string Name { get; set; }
    public List<Employee> Employees { get; set; } = new();
}