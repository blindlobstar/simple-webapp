using System.Text.Json.Serialization;
using SimpleWebApp.Domain.Entities;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.Dto.Employees;

public sealed class EmployeeDto
{
    [JsonConstructor]
    public EmployeeDto(Guid id, EmployeeTitle title, string email, Guid[] companies)
    {
        Id = id;
        Title = title;
        Email = email;
        Companies = companies;
    }

    public EmployeeDto(Employee employee)
    {
        Id = employee.Id;
        Title = employee.Title;
        Email = employee.Email;
        Companies = employee.Companies.Select(x => x.Id).ToArray();
    }

    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("title")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EmployeeTitle Title { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("companies")]
    public Guid[] Companies { get; set; }
}