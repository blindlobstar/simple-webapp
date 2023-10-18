namespace SimpleWebApp.Api.Dto.Employees;

public sealed class TitleExistsErrorResponse : ErrorResponse<Guid[]>
{
    public TitleExistsErrorResponse(Guid[] companyIds)
    {
       Details = companyIds; 
    }

    public override uint Code => 20001;
    public override string ErrorMessage => "One or more company already have employee with provided title";
    public override Guid[] Details { get; protected set; }
}
