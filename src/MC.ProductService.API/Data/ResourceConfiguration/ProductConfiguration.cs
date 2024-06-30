using MC.ProductService.API.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MC.ProductService.API.Data.ResourceConfiguration
{
    /// <summary>
    /// Configures entity mapping for the <see cref="Product"/> class specifically tailored for PostgreSQL.
    /// This configuration extends <see cref="PostgresResourceConfiguration{Product}"/> to include
    /// additional indexing specific to the Product entity.
    /// </summary>
    public class ProductConfiguration : PostgresResourceConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(b => b.ProductId).HasConversion<Guid>();
        }
    }
}
