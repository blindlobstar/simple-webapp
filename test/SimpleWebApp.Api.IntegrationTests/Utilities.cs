using System.Data.Common;
using SimpleWebApp.Data;
using SimpleWebApp.Domain.Entities;
using SimpleWebApp.Domain.Enums;

namespace SimpleWebApp.Api.IntegrationTests;

public static class Utilities
{
    public static void Seed(SimpleDBContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.Companies.AddRange(GetSeedingCompanies());
        dbContext.SaveChanges();
    }
    public static Company[] GetSeedingCompanies() => 
        new Company[]
        { 
            new()
            {
                Id = Guid.Parse("a99d6cc1-9150-4caf-8d8d-571c5ab76782"),
                Name = "Microsoft"
            },
            new()
            {
                Id = Guid.Parse("3d2b01f9-56c9-4b2e-b678-0f56019588ff"),
                Name = "Google",
                Employees = new()
                {
                    new()
                    {
                        Id = Guid.Parse("1e5a51fe-e726-48e8-9fed-0af3121dca80"),
                        Title = EmployeeTitle.Manager,
                        Email = "manager@google.com",
                    }
                }
            },
            new()
            {
                Id = Guid.Parse("bcb71cb2-e016-478b-bc37-ea9909c263ac"),
                Name = "Apple",
                Employees = new()
                {
                    new()
                    {
                        Id = Guid.Parse("7287d91a-68cc-4541-a5c3-c97d41d966c1"),
                        Title = EmployeeTitle.Developer,
                        Email = "developer@apple.com",
                    } 
                }
            }
        };
}