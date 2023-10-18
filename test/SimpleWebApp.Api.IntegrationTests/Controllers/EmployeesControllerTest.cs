using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApp.Api.Dto.Employees;
using SimpleWebApp.Data;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.IntegrationTests.Controllers;

[TestFixture]
public sealed class EmployeesControllerTest
{
    private readonly SimpleApiWebApplicationFactory _webApplicationFactory;

    public EmployeesControllerTest()
    {
        _webApplicationFactory = new SimpleApiWebApplicationFactory();
    }

    [Test]
    public async Task CreateEmployee_EmployeeCreated_200()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateEmployeeRequest
        {
            Email = "tester@google.com",
            Title = EmployeeTitle.Tester,
            Companies = new Guid[]
            {
                Utilities.GetSeedingCompanies()[0].Id
            }
        };
        var response = await client.PostAsJsonAsync("/api/employees", createRequest);

        response.EnsureSuccessStatusCode();

        var employee = await response.Content.ReadFromJsonAsync<EmployeeDto>();
        Assert.That(employee, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(employee!.Email, Is.EqualTo(createRequest.Email));
            Assert.That(employee!.Companies[0], Is.EqualTo(createRequest.Companies[0]));
        });
    }

    [Test]
    public async Task CreateEmployee_ValidationError_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            dbContext.Database.EnsureCreated();
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateEmployeeRequest
        {
            Email = "notAEmail",
            Title = EmployeeTitle.Developer
        };
        var response = await client.PostAsJsonAsync("/api/employees", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(30001));
    }

    [Test]
    public async Task CreateEmployee_EmailMustBeUnique_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateEmployeeRequest
        {
            Email = Utilities.GetSeedingCompanies()[1].Employees[0].Email,
            Title = EmployeeTitle.Developer,
            Companies = new Guid[] { Utilities.GetSeedingCompanies()[0].Id }
        };
        var response = await client.PostAsJsonAsync("/api/employees", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(20002));
    }

    [Test]
    public async Task CreateEmployee_CompanyNotExistError_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            dbContext.Database.EnsureCreated();
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateEmployeeRequest
        {
            Email = "developer@testmail.com",
            Title = EmployeeTitle.Developer,
            Companies = new Guid[] { Guid.NewGuid() }
        };
        var response = await client.PostAsJsonAsync("/api/employees", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(20000));
    }

    [Test]
    public async Task CreateEmployee_TitleConflict_400()
    {
        using (var scope = _webApplicationFactory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SimpleDBContext>();
            Utilities.Seed(dbContext);
        }

        var client = _webApplicationFactory.CreateClient();
        var createRequest = new CreateEmployeeRequest
        {
            Email = "developer@testmail.com",
            Title = EmployeeTitle.Developer,
            Companies = new Guid[] { Utilities.GetSeedingCompanies()[2].Id }
        };
        var response = await client.PostAsJsonAsync("/api/employees", createRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        var error = await response.Content.ReadFromJsonAsync<JsonNode>();
        Assert.That(error, Is.Not.Null);
        Assert.That(error!["code"]!.GetValue<int>(), Is.EqualTo(20001));
    }
}