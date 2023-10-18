using System.Text.Json.Serialization;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Api.Dto.Companies;

public sealed class CompanyDto
{
    [JsonConstructor]
    public CompanyDto(
        Guid id,
        string name,
        CompanyEmployee[] employees)
    {
        Id = id;
        Name = name;
        Employees = employees;
    }

    public CompanyDto(Company company)
    {
        Id = company.Id;
        Name = company.Name;
        Employees = company.Employees
            .Select(x => new CompanyEmployee { Id = x.Id, Email = x.Email, Title = x.Title })
            .ToArray();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public CompanyEmployee[] Employees { get; set; }
}