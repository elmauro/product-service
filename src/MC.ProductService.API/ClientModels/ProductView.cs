using MC.ProductService.API.Data.Models;
using System.Linq.Expressions;

namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// Provides a lambda expression that projects a <see cref="Product"/> entity to a <see cref="ProductView"/> model.
    /// This static method is typically used for transforming database entities into more suitable formats
    /// for client-facing applications, ensuring that sensitive or irrelevant entity data is not exposed.
    /// </summary>
    /// <returns>
    /// An expression that maps properties from the <see cref="Product"/> entity to the <see cref="ProductView"/> model,
    /// enabling efficient querying and data transformation directly in database queries.
    /// </returns>
    public class ProductView
    {
        /// <see cref="Product.Name"/>
        public string Name { get; set; }

        /// <see cref="Product.Status"/>
        public int Status { get; set; }

        /// <see cref="Product.Stock"/>
        public int Stock { get; set; }

        /// <see cref="Product.Description"/>
        public string Description { get; set; }

        /// <see cref="Product.Price"/>
        public int Price { get; set; }

        /// <see cref="Product.ProductId"/>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the status name of the product. This is a calculated field.
        /// </summary>
        /// <value>The status name of the product.</value>
        public string StatusName { get; set; }

        /// <summary>
        /// Gets or sets the discount applied to the product. This is a percentage value that represents
        /// the reduction from the original price. This is a calculated field.
        /// </summary>
        /// <value>
        /// The discount percentage applied to the product price.
        /// </value>
        public int Discount { get; set; }

        /// <summary>
        /// Gets or sets the final price of the product after applying the discount. This value accounts
        /// for any reductions through discounts and is the amount that the customer would be expected to pay. 
        /// This is a calculated field.
        /// </summary>
        /// <value>
        /// The final price of the product after discount.
        /// </value>
        public decimal FinalPrice { get; set; }

        public static Expression<Func<Product, ProductView>> Project() => product => new ProductView
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Status = product.Status,
            Stock = product.Stock,
            Description = product.Description,
            Price = product.Price
        };
    }
}
