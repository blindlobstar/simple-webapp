namespace SimpleWebApp.Api.Dto.Companies;

public sealed class EmployeeNotExistsErrorResponse : ErrorResponse<Guid[]>
{
    public EmployeeNotExistsErrorResponse(Guid[] employees)
    {
        Details = employees;
    }

    public override uint Code => 10000;
    public override string ErrorMessage => "One or more of the employees do not exist";
    public override Guid[] Details { get; protected set; }
}
