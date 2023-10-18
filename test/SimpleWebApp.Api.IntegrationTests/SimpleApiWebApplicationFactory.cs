using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using SimpleWebApp.Data;

namespace SimpleWebApp.Api.IntegrationTests;

public sealed class SimpleApiWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<SimpleDBContext>));

            services.Remove(dbContextDescriptor!);

            services.AddDbContext<SimpleDBContext>(opt => 
                opt
                    .AddInterceptors(new IgnoringIdentityResolutionInterceptor())
                    .UseNpgsql(Environment.GetEnvironmentVariable("TestConnectionString")));
        });

    }
}