using System.Text.Json.Serialization;

namespace SimpleWebApp.Api.Dto;

public abstract class ErrorResponse<TDetails> : ErrorResponse
{
    [JsonPropertyName("details")]
    public abstract TDetails Details { get; protected set; }
}

public abstract class ErrorResponse
{
    [JsonPropertyName("code")]
    public abstract uint Code { get; }

    [JsonPropertyName("errorMessage")]
    public abstract string ErrorMessage { get; }
}