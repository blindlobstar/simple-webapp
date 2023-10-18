namespace SimpleWebApp.Api.Dto;

public sealed class ValidationErrorResponse : ErrorResponse<IDictionary<string, string[]>>
{
    public ValidationErrorResponse(IDictionary<string, string[]> details)
    {
        Details = details;
    }

    public override uint Code => 30001;
    public override string ErrorMessage => "Validation error";
    public override IDictionary<string, string[]> Details { get; protected set; }
}
