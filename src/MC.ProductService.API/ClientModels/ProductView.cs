using MC.ProductService.API.Data.Models;
using System.Linq.Expressions;

namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// This class creates a way to turn a Product into a ProductView model.
    /// It takes the details from a Product and sets up a ProductView with those same details.
    /// </summary>
    /// <returns>
    /// A formula that helps convert a Product's data into a ProductView format.
    /// </returns>
    public class ProductView
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The current state of the product, like whether it's active or not.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// How many units of the product are available.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// A description of what the product is.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The price of the product.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The unique code assigned to the product.
        /// </summary>
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

        /// <summary>
        /// The ID of the person who first added the product to the system.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the last person who updated the product's details.
        /// </summary>
        public string LastUpdatedBy { get; set; } = string.Empty;

        /// <summary>
        /// When the product was first added to the system.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The last time the product's details were updated.
        /// </summary>
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
