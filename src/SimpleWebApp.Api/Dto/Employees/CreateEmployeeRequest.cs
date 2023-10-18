using System.Text.Json.Serialization;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.Dto.Employees;

public sealed class CreateEmployeeRequest
{
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("title")]
    public required EmployeeTitle Title { get; set; }

    [JsonPropertyName("companies")]
    public Guid[] Companies { get; set; } = Array.Empty<Guid>();
}