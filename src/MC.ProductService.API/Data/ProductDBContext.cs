using Microsoft.EntityFrameworkCore;
using MC.ProductService.API.Data.Models;

namespace MC.ProductService.API.Data
{
    /// <summary>
    /// Provides context for product database interactions, encapsulating configuration
    /// and functionality for accessing the database through Entity Framework Core.
    /// </summary>
    /// <remarks>
    /// This context is configured to use specific database options provided during instantiation
    /// and applies entity configurations dynamically from all configurations defined in the assembly.
    /// </remarks>
    public class ProductDBContext(DbContextOptions<ProductDBContext> options) : DbContext(options)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDBContext"/> class.
        /// </summary>
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .HasAnnotation("Relational:Collation", "en_US.utf8")
                .ApplyConfigurationsFromAssembly(typeof(ProductDBContext).Assembly);
        }
    }
}
