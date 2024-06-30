using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MC.ProductService.API.Data; // Ensure the namespace is correct

namespace MC.ProductService.Tests.Fixtures
{
    /// <summary>
    /// Factory for bootstrapping an application for integration tests,
    /// with methods for setting up a clean database.
    /// Inherits from WebApplicationFactory with the startup class, Program.
    /// </summary>
    public class ProductApplicationFactory : WebApplicationFactory<Program>
    {
        // <summary>
        /// Configures the web host for testing. This method is called by the constructor of WebApplicationFactory.
        /// It modifies the IServiceCollection used by the application.
        /// </summary>
        /// <param name="builder">The IWebHostBuilder used to create the host.</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Search for the existing DbContextOptions for ProductDBContext within the service collection
                // in order to replace it with a new service definition for integration testing purposes.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProductDBContext>));

                // If the descriptor is found, it is removed from the collection to prevent the actual database from being used.
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
            });
        }
    }
}
