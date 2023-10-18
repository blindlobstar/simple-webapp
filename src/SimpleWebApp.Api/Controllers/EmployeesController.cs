using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApp.Api.Dto;
using SimpleWebApp.Api.Dto.Employees;
using SimpleWebApp.Data.Repositories;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Api.Controllers;

[ApiController]
[Route("/api/employees")]
public sealed class EmployeesController : ControllerBase
{
    private readonly IValidator<CreateEmployeeRequest> _validator;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICompanyRepository _companyRepository;

    public EmployeesController(
        IEmployeeRepository employeeRepository,
        ICompanyRepository companyRepository,
        IValidator<CreateEmployeeRequest> validator)
    {
        _employeeRepository = employeeRepository;
        _companyRepository = companyRepository;
        _validator = validator;
    }

    [HttpPost("")]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid) 
        {
            return BadRequest(new ValidationErrorResponse(validationResult.ToDictionary()));
        }

        if (await _employeeRepository.IsEmployeeExistsAsync(request.Email))
        {
            return BadRequest(new EmailMustBeUniqueErrorResponse());
        }
        
        var companies = await _companyRepository.GetCompaniesWithEmployeesAsync(request.Companies);
        
        var nonExistsCompanies = request.Companies.Where(x => !companies.Select(x => x.Id).Contains(x)).ToArray();
        if (nonExistsCompanies.Length > 0)
        {
            return BadRequest(new CompanyNotExistsErrorResponse(nonExistsCompanies));
        }

        var conflictCompanies = companies
            .SelectMany(
                x => x.Employees, 
                (company, employee) => new { CompanyId = company.Id, EmployeeTitle = employee.Title})
            .Where(x => x.EmployeeTitle == request.Title).ToArray();
        if (conflictCompanies.Length > 0)
        {
            return BadRequest(new TitleExistsErrorResponse(conflictCompanies.Select(x => x.CompanyId).ToArray()));
        }

        var employee = new Employee
        {
            Email = request.Email,
            Title = request.Title
        };
        employee.Companies.AddRange(companies);
        await _employeeRepository.CreateAsync(employee);

        return Ok(new EmployeeDto(employee));
    }
}