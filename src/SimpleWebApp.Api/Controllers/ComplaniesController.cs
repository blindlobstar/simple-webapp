using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Api.Dto;
using SimpleWebApp.Api.Dto.Companies;
using SimpleWebApp.Data;
using SimpleWebApp.Data.Repositories;
using SimpleWebApp.Domain.Entities;

namespace SimpleWebApp.Api.Controllers;

[ApiController]
[Route("/api/companies")]
public sealed class CompaniesController : ControllerBase
{
    private readonly IValidator<CreateCompanyRequest> _validator;
    private readonly ICompanyRepository _companyRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly SimpleDBContext _dbContext;

    public CompaniesController(
        ICompanyRepository companyRepository,
        IEmployeeRepository employeeRepository,
        SimpleDBContext dbContext,
        IValidator<CreateCompanyRequest> validator)
    {
        _companyRepository = companyRepository;
        _employeeRepository = employeeRepository;
        _dbContext = dbContext;
        _validator = validator;
    }

    [HttpPost]
    public async Task<ActionResult<CompanyDto>> Create(CreateCompanyRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) 
        {
            return BadRequest(new ValidationErrorResponse(validationResult.ToDictionary()));
        }

        if (await _companyRepository.IsCompanyExistsAsync(request.Name, cancellationToken))
        {
            return BadRequest(new NameMustBeUniqueErrorResponse());
        }

        var existingEmployeesIds = request.Employees.Where(x => x.Id.HasValue).Select(x => x.Id!.Value);
        var existingEmployees = await _employeeRepository.GetEmployeesAsync(existingEmployeesIds, cancellationToken); 
        
        var notExistingEmployeesIds = existingEmployeesIds.Where(x => !existingEmployees.Select(x => x.Id).Contains(x)).ToArray();
        if (notExistingEmployeesIds.Length > 0)
        {
            return BadRequest(new EmployeeNotExistsErrorResponse(notExistingEmployeesIds));
        }

        var newEmployees = request.Employees.Where(x => !x.Id.HasValue);

        var nonUniqueEmails = existingEmployees
            .Select(x => x.Email)
            .Concat(newEmployees.Select(x => x.Email))
            .GroupBy(x => x)
            .Where(x => x.Count() > 1)
            .Select(x => x.Key!)
            .ToArray();
        if (nonUniqueEmails.Length > 0)
        {
            return BadRequest(new EmployeeEmailMustBeUniqueErrorResponse(nonUniqueEmails));
        }

        var nonDistinctTitles = existingEmployees
            .Select(x => new CompanyEmployee{ Id = x.Id, Title = x.Title, Email = x.Email })
            .Concat(newEmployees)
            .GroupBy(x => x.Title)
            .Where(x => x.Count() > 1)
            .ToDictionary(x => x.Key!.Value, x => x.ToArray());
        if (nonDistinctTitles.Count > 0)
        {
            return BadRequest(new EmployeeTitleConflictErrorResponse(nonDistinctTitles));
        }

        var existingEmails = new List<string>();
        foreach (var newEmployee in newEmployees) 
        {
           if (await _employeeRepository.IsEmployeeExistsAsync(newEmployee.Email!, cancellationToken: cancellationToken))
           {
                existingEmails.Add(newEmployee.Email!);
           } 
        }
        if (existingEmails.Count > 0)
        {
           return BadRequest(new EmployeeEmailMustBeUniqueErrorResponse(existingEmails.ToArray())); 
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        var company = new Company
        {
            Name = request.Name,
        };
        company.Employees.AddRange(existingEmployees);
        await _companyRepository.CreateAsync(company, cancellationToken);
        
        foreach (var employeeToCreate in newEmployees)
        {
            var employee = new Employee
            {
                Email = employeeToCreate.Email!,
                Title = employeeToCreate.Title!.Value,
                Companies = new() { company }
            };
            await _employeeRepository.CreateAsync(employee, cancellationToken);
        }

        await transaction.CommitAsync(cancellationToken);

        return Ok(new CompanyDto(company));
    }
}