using MC.ProductService.API.Data.Models;
using System.Linq.Expressions;

namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// Projects a <see cref="Product"/> entity to a <see cref="ProductView"/> model.
    /// </summary>
    /// <returns>
    /// Helps to convert a Product's data into a ProductView format.
    /// </returns>
    public class ProductView
    {
        /// <see cref="Product.Name"/>
        public string Name { get; set; } = string.Empty;

        /// <see cref="Product.Status"/>
        public int Status { get; set; }

        /// <see cref="Product.Stock"/>
        public int Stock { get; set; }

        /// <see cref="Product.Description"/>
        public string Description { get; set; } = string.Empty;

        /// <see cref="Product.Price"/>
        public int Price { get; set; }

        /// <see cref="Product.ProductId"/>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// A word or phrase that describes the product's current status.
        /// </summary>
        public string StatusName { get; set; } = string.Empty;

        /// <summary>
        /// The percentage off the original price as a discount.
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// The price of the product after the discount has been applied.
        /// </summary>
        public decimal FinalPrice { get; set; }

        /// <see cref="Product.CreatedBy"/>
        public string CreatedBy { get; set; } = string.Empty;

        /// <see cref="Product.LastUpdatedBy"/>
        public string LastUpdatedBy { get; set; } = string.Empty;

        /// <see cref="Product.CreatedAt"/>
        public DateTimeOffset CreatedAt { get; set; }

        /// <see cref="Product.LastUpdatedAt"/>
        public DateTimeOffset LastUpdatedAt { get; set; }

        /// <summary>
        /// Provides a way to automatically create a ProductView from a Product.
        /// </summary>
        public static Expression<Func<Product, ProductView>> Project() => product => new ProductView
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Status = product.Status,
            Stock = product.Stock,
            Description = product.Description,
            Price = product.Price,
            CreatedBy = product.CreatedBy,
            LastUpdatedBy = product.LastUpdatedBy,
            CreatedAt = product.CreatedAt,
            LastUpdatedAt = product.LastUpdatedAt
        };
    }
}
