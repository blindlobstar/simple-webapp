using System.Text.Json.Serialization;

namespace SimpleWebApp.Api.Dto.Companies;

public sealed class CreateCompanyRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("employees")]
    public CompanyEmployee[] Employees { get; set; } = Array.Empty<CompanyEmployee>();
}
