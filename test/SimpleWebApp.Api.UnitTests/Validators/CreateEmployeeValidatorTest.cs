using FluentValidation.TestHelper;
using SimpleWebApp.Api.Dto.Employees;
using SimpleWebApp.Api.Validators;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.UnitTests.Validators;

[TestFixture]
public sealed class CreateEmployeeValidatorTest
{
    private readonly CreateEmployeeValidator _validator;

    public CreateEmployeeValidatorTest()
    {
        _validator = new();
    }
    [Test]
    public void Title_NotEmpty()
    {
        _validator.TestValidate(new CreateEmployeeRequest { Title = EmployeeTitle.None, Email = "test@mail.com" })
            .ShouldHaveValidationErrorFor(x => x.Title);
        
        _validator.TestValidate(new CreateEmployeeRequest { Title = EmployeeTitle.Developer, Email = "test@mail.com"})
            .ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Email_NotEmptyAndValid()
    {
        _validator.TestValidate(new CreateEmployeeRequest { Title = EmployeeTitle.Developer, Email = "" })
            .ShouldHaveValidationErrorFor(x => x.Email);
        _validator.TestValidate(new CreateEmployeeRequest { Title = EmployeeTitle.Developer, Email = "invalidemail" })
            .ShouldHaveValidationErrorFor(x => x.Email);
        _validator.TestValidate(new CreateEmployeeRequest { Title = EmployeeTitle.Developer, Email = "test@mail.com"})
            .ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Companies_NotEmpty()
    {
        _validator.TestValidate(new CreateEmployeeRequest { Title = EmployeeTitle.Developer, Email = "" })
            .ShouldHaveValidationErrorFor(x => x.Companies);
        _validator.TestValidate(new CreateEmployeeRequest
        {
            Title = EmployeeTitle.Developer,
            Email = "",
            Companies = new Guid[] { Guid.NewGuid() }
        }).ShouldNotHaveValidationErrorFor(x => x.Companies);
    }
}