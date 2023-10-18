using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApp.Api.Dto.Companies;
using SimpleWebApp.Data;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.IntegrationTests.Controllers;

[TestFixture]
public sealed class CompaniesControllerTest
{
    private readonly SimpleApiWebApplicationFactory _webApplicationFactory;

    public CompaniesControllerTest()
    {
        _webApplicationFactory = new SimpleApiWebApplicationFactory();
    }

    [Test]
    public async Task CreateCompany_CompanyCreated()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateCompanyRequest
        {
            Name = "test company",
            Employees = new CompanyEmployee[]
            {
                new() { Title = EmployeeTitle.Developer, Email = "developer@testmail.com" },
                new() { Title = EmployeeTitle.Tester, Email = "tester@tesmail.com" },
                new() { Id = Utilities.GetSeedingCompanies()[1].Employees[0].Id }
            }
        };
        var response = await client.PostAsJsonAsync("/api/companies", createRequest);

        response.EnsureSuccessStatusCode();

        var company = await response.Content.ReadFromJsonAsync<CompanyDto>();
        Assert.That(company, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(company!.Name, Is.EqualTo(createRequest.Name));
            Assert.That(company!.Employees, Has.Length.EqualTo(createRequest.Employees.Length));
        });
    }

    [Test]
    public async Task CreateCompany_ValidationError_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            dbContext.Database.EnsureCreated();
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateCompanyRequest
        {
            Name = "test company",
            Employees = new CompanyEmployee[]
            {
                new() { Title = EmployeeTitle.Developer, Email = "notAMail" },
            }
        };
        var response = await client.PostAsJsonAsync("/api/companies", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(30001));
    }

    [Test]
    public async Task CreateCompany_NameMustBeUnique_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateCompanyRequest
        {
            Name = Utilities.GetSeedingCompanies()[0].Name,
            Employees = new CompanyEmployee[]
            {
                new() { Title = EmployeeTitle.Tester, Email = "tester@tesmail.com" },
            }
        };
        var response = await client.PostAsJsonAsync("/api/companies", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(10002));
    }

    [Test]
    public async Task CreateCompany_NonExistingEmployee_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            dbContext.Database.EnsureCreated();
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateCompanyRequest
        {
            Name = "test company",
            Employees = new CompanyEmployee[]
            {
                new() { Id = Guid.NewGuid() },
            }
        };
        var response = await client.PostAsJsonAsync("/api/companies", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(10000));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task CreateCompany_EmployeeTitleConflict_400(bool withExisting)
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        CompanyEmployee[] employees = withExisting
            ? new CompanyEmployee[]
            {
                new() { Id = Utilities.GetSeedingCompanies()[1].Employees[0].Id },
                new() { Email = "manager@testmail.com", Title = EmployeeTitle.Manager }
            }
            : new CompanyEmployee[]
            {
                new() { Email = "tester@testmail.com", Title = EmployeeTitle.Tester},
                new() { Email = "newtester@testmail.com", Title = EmployeeTitle.Tester }
            };
        var createRequest = new CreateCompanyRequest
        {
            Name = "test company",
            Employees = employees
        };
        var response = await client.PostAsJsonAsync("/api/companies", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(10001));
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public async Task CreateCompany_EmployeeEmailMustBeUnique_400(bool withExisting)
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        CompanyEmployee[] employees = withExisting
            ? new CompanyEmployee[]
            {
                new() { Email = "manager@google.com", Title = EmployeeTitle.Developer }
            }
            : new CompanyEmployee[]
            {
                new() { Email = "same@testmail.com", Title = EmployeeTitle.Developer },
                new() { Email = "same@testmail.com", Title = EmployeeTitle.Tester }
            };
        var createRequest = new CreateCompanyRequest
        {
            Name = "test company",
            Employees = employees
        };
        var response = await client.PostAsJsonAsync("/api/companies", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(10003));
    }
}