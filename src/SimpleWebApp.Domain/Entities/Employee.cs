using SimpleWebApp.Core.Domain.Entities;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Domain.Entities;

public class Employee : EntityBase<Guid>
{
    public EmployeeTitle Title { get; set; }
    public required string Email { get; set; }
    public List<Company> Companies { get; set; } = new();
}