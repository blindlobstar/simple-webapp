using Microsoft.EntityFrameworkCore;
using SimpleWebApp.Data;
using SimpleWebApp.Data.Repositories;
using SimpleWebApp.Data.Repositories.Implementation;
using SimpleWebApp.Data.SysLog.Mappers.Implementation;
using SimpleWebApp.Data.SysLog;
using SimpleWebApp.Domain.Entities;
using FluentValidation;
using SimpleWebApp.Api.Dto.Companies;
using SimpleWebApp.Api.Validators;
using SimpleWebApp.Api.Dto.Employees;

var builder = WebApplication.CreateBuilder(args);

// Data
builder.Services.AddDbContext<SimpleDBContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("SimpleDB"));
});

builder.Services.AddLoggedRepository<IEmployeeRepository, EmployeeRepository, EmployeeSysLogMapper, Employee, Guid>();
builder.Services.AddLoggedRepository<ICompanyRepository, CompanyRepository, CompanySysLogMapper, Company, Guid>();


// Application
builder.Services.AddScoped<IValidator<CreateEmployeeRequest>, CreateEmployeeValidator>();
builder.Services.AddScoped<IValidator<CreateCompanyRequest>, CreateCompanyValidator>();

// Infrastructure 
builder.Services.AddControllers();
builder.Services.AddOpenApiDocument();


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();

app.Run();

public partial class Program { }