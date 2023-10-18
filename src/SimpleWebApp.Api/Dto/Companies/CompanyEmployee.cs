using System.Text.Json.Serialization;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.Dto.Companies;

public sealed class CompanyEmployee
{
    [JsonPropertyName("id")]
    public Guid? Id { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("title")]
    public EmployeeTitle? Title { get; set; }
}