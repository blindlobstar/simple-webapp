namespace SimpleWebApp.Api.Dto.Companies;

public sealed class NameMustBeUniqueErrorResponse : ErrorResponse
{
    public override uint Code => 10002;

    public override string ErrorMessage => "Name must be unique";
}
