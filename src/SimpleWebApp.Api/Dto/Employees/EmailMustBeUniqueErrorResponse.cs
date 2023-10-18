namespace SimpleWebApp.Api.Dto.Employees;

public sealed class EmailMustBeUniqueErrorResponse : ErrorResponse
{
    public override uint Code => 20002;

    public override string ErrorMessage => "Employee email must be unique";
}
