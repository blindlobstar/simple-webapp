using FluentValidation.TestHelper;
using SimpleWebApp.Api.Dto.Companies;
using SimpleWebApp.Api.Validators;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.UnitTests.Validators;

[TestFixture]
public sealed class CompanyEmployeeValidatorTest
{
    private readonly CompanyEmployeeValidator _validator;

    public CompanyEmployeeValidatorTest()
    {
        _validator = new();
    }

    [Test]
    public void Id_NotEmpty_OneOf()
    {
        _validator.TestValidate(new CompanyEmployee()
        {
            Id = null,
        }).ShouldHaveValidationErrorFor(x => x.Id);

        _validator.TestValidate(new CompanyEmployee()
        {
            Id = Guid.NewGuid(),
            Title = EmployeeTitle.Developer
        }).ShouldHaveValidationErrorFor(x => x.Id);
        
        _validator.TestValidate(new CompanyEmployee()
        {
            Id = Guid.NewGuid(),
            Email = "valid@mail.com"
        }).ShouldHaveValidationErrorFor(x => x.Id);

        _validator.TestValidate(new CompanyEmployee()
        {
            Id = Guid.NewGuid(),
        }).ShouldNotHaveValidationErrorFor(x => x.Id);
    }

    [Test]
    public void Title_NotEmpty()
    {
        _validator.TestValidate(new CompanyEmployee()
        {
            Title = EmployeeTitle.None,
            Email = "valid@mail.com"
        }).ShouldHaveValidationErrorFor(x => x.Title);

        _validator.TestValidate(new CompanyEmployee()
        {
            Email = "valid@mail.com"
        }).ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Email_NotEmpty_Valid()
    {
        _validator.TestValidate(new CompanyEmployee()
        {
            Title = EmployeeTitle.Manager,
            Email = "invalid_email"
        }).ShouldHaveValidationErrorFor(x => x.Email);

        _validator.TestValidate(new CompanyEmployee()
        {
            Title = EmployeeTitle.Manager,
        }).ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Test]
    public void Email_And_Title_No_Errors()
    {
        _validator.TestValidate(new CompanyEmployee()
        {
            Title = EmployeeTitle.Manager,
            Email = "valid@mail.com"
        }).ShouldNotHaveAnyValidationErrors();

    }
}