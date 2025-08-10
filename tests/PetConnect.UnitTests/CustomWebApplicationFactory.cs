using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetConnect.API;
using PetConnect.DAL.Data;


namespace PetConnect.UnitTests
{
    // This class creates a special version of our API application for integration testing
    // It lets us change how services are configured so we can replace real DB with an in-memory DB
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        // This method runs when the test server is being built
        // We override it to change the environment and the DB setup
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Keep base behavior (important so the default setup still works)
            base.ConfigureWebHost(builder);
            // Set the environment to "Test" so it can load test-specific settings (like appsettings.Test.json)
            builder.UseEnvironment("Test");
            // Change the service registrations
            builder.ConfigureServices(services =>
            {
                // Find the existing AppDbContext registration (using SQL Server)
                var descriptor = services.SingleOrDefault(temp => temp.ServiceType == typeof(DbContextOptions<AppDbContext>));
                // Remove it so we can replace it with our in-memory DB
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }
                // Add our in-memory database instead of the real one
                // This makes tests faster and avoids touching real data
                services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestingDataBase"));
            });
        }
    }
}
