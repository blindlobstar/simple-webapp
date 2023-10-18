namespace SimpleWebApp.Api.Dto.Companies;

public sealed class EmployeeNotExistsErrorResponse : ErrorResponse<Guid[]>
{
    public EmployeeNotExistsErrorResponse(Guid[] employees)
    {
        Details = employees;
    }

    public override uint Code => 10000;
    public override string ErrorMessage => "Trying to add non existing employee";
    public override Guid[] Details { get; protected set; }
}
