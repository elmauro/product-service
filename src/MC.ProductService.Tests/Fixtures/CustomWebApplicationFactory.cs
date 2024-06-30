using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using MC.ProductService.API.Data; // Ensure the namespace is correct

namespace MC.ProductService.Tests.Fixtures
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing context configuration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ProductDBContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }
            });
        }

        private void InitializeDbForTests(ProductDBContext db)
        {
            // Seed database logic here
        }
    }
}
