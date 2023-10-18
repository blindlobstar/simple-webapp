using FluentValidation;
using SimpleWebApp.Api.Dto.Employees;

namespace SimpleWebApp.Api.Validators;

public sealed class CreateEmployeeValidator : AbstractValidator<CreateEmployeeRequest>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.Title).NotEmpty().NotEqual(Domain.Enums.EmployeeTitle.None);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Companies).NotEmpty();
    }
}