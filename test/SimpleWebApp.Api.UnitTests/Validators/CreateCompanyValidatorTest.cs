using FluentValidation.TestHelper;
using SimpleWebApp.Api.Dto.Companies;
using SimpleWebApp.Api.Validators;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.UnitTests.Validators;

[TestFixture]
public sealed class CreateCompanyValidatorTest
{
    private readonly CreateCompanyValidator _validator;

    public CreateCompanyValidatorTest()
    {
        _validator = new();
    }

    [Test]
    public void Name_NotEmpty()
    {
        _validator.TestValidate(new CreateCompanyRequest { Name = ""})
            .ShouldHaveValidationErrorFor(x => x.Name);
        
        _validator.TestValidate(new CreateCompanyRequest { Name = "name" })
            .ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Test]
    public void Employees_NotEmpty()
    {
        _validator.TestValidate(new CreateCompanyRequest { Name = "name"})
            .ShouldHaveValidationErrorFor(x => x.Employees);
        
        _validator.TestValidate(new CreateCompanyRequest
        {
            Name = "name",
            Employees = new CompanyEmployee[] { new() { Id = Guid.NewGuid() } }
        }).ShouldNotHaveAnyValidationErrors();

        _validator.TestValidate(new CreateCompanyRequest
        {
            Name = "name",
            Employees = new CompanyEmployee[] { new() { Title = EmployeeTitle.Developer, Email = "valid@email.com" } }
        }).ShouldNotHaveAnyValidationErrors();
    }
}