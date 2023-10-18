namespace SimpleWebApp.Api.Dto.Employees;

public sealed class CompanyNotExistsErrorResponse : ErrorResponse<Guid[]>
{
    public CompanyNotExistsErrorResponse(Guid[] companyIds)
    {
        Details = companyIds;
    }

    public override uint Code => 20000;

    public override string ErrorMessage => "One or more of the companies do not exist";

    public override Guid[] Details { get; protected set; }
}
