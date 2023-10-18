using FluentValidation;
using SimpleWebApp.Api.Dto.Companies;

namespace SimpleWebApp.Api.Validators;

public sealed class CompanyEmployeeValidator : AbstractValidator<CompanyEmployee>
{
    public CompanyEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty().When(x => x.Email == null && x.Title == null);
        RuleFor(x => x.Id).Empty().When(x => x.Email != null || x.Title != null)
            .WithMessage("id or title and email");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().When(x => !x.Id.HasValue);
        RuleFor(x => x.Title).NotEmpty().NotEqual(Domain.Enums.EmployeeTitle.None).When(x => !x.Id.HasValue);
    }
}