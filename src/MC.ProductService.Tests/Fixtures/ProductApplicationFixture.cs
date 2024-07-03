using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MC.ProductService.API.Data; // Ensure the namespace is correct

namespace MC.ProductService.Tests.Fixtures
{
    /// <summary>
    /// A setup tool for creating an app environment for integration tests.
    /// It includes methods to start with a fresh database each time.
    /// This class extends WebApplicationFactory and uses the startup configuration from Program.
    /// </summary>
    public class ProductApplicationFixture : WebApplicationFactory<Program>
    {
        /// <summary>
        /// Sets up the web host for testing. This method gets called automatically 
        /// when creating an instance of WebApplicationFactory.
        /// It changes the service configuration used by the app.
        /// </summary>
        /// <param name="builder">The builder that sets up the web host.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Look for any database configurations for ProductDBContext already registered.
                // The goal is to replace them to avoid using the actual production database during tests.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProductDBContext>));

                // If a configuration is found, remove it. This ensures the test doesn't use the production database.
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
            });
        }
    }
}
