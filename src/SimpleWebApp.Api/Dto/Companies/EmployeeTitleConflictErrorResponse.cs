using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.Dto.Companies;

public sealed class EmployeeTitleConflictErrorResponse : ErrorResponse<Dictionary<EmployeeTitle, CompanyEmployee[]>>
{
    public EmployeeTitleConflictErrorResponse(Dictionary<EmployeeTitle, CompanyEmployee[]> details)
    {
        Details = details;
    }

    public override uint Code => 10001;
    public override string ErrorMessage => "Employee title conflict";
    public override Dictionary<EmployeeTitle, CompanyEmployee[]> Details { get; protected set; }
}