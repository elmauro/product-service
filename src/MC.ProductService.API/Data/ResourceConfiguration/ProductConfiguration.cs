using MC.ProductService.API.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MC.ProductService.API.Data.ResourceConfiguration
{
    /// <summary>
    /// Sets up how product data is saved in the database.
    /// This setup includes special rules for products, like how to save product IDs.
    /// </summary>
    public class ProductConfiguration : PostgresResourceConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            // Use basic settings from the general PostgreSQL configuration for products
            base.Configure(builder);

            // Make sure the product ID is saved as a GUID (a type of unique identifier) in the database.
            builder.Property(b => b.ProductId).HasConversion<Guid>();
        }
    }
}
