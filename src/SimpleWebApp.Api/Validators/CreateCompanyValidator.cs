using FluentValidation;
using SimpleWebApp.Api.Dto.Companies;

namespace SimpleWebApp.Api.Validators;

public sealed class CreateCompanyValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyValidator()
    {
       RuleFor(x => x.Name).NotEmpty(); 
       RuleFor(x => x.Employees).NotEmpty();
       RuleForEach(x => x.Employees).SetValidator(new CompanyEmployeeValidator());
    }
}