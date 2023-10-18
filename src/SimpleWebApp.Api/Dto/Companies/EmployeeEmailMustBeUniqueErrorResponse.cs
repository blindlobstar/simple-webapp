namespace SimpleWebApp.Api.Dto.Companies;

public sealed class EmployeeEmailMustBeUniqueErrorResponse : ErrorResponse<string[]>
{
    public EmployeeEmailMustBeUniqueErrorResponse(string[] emails)
    {
        Details = emails;
    }

    public override uint Code => 10003;
    public override string ErrorMessage => "Employees' email must be unique";
    public override string[] Details { get; protected set; }
}
