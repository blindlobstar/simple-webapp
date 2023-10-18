using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SimpleWebApp.Data.Migrations;

public sealed class SimpleDBContextFactory : IDesignTimeDbContextFactory<SimpleDBContext>
{
    public SimpleDBContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SimpleDBContext>()
            .UseNpgsql(
                Environment.GetEnvironmentVariable("ConnectionString__Migration"), 
                opt => opt
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name))
            .Options;

        return new SimpleDBContext(options);
    }
}
