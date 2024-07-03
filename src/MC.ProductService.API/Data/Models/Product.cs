using MC.ProductService.API.Data.ResourceConfiguration;

namespace MC.ProductService.API.Data.Models
{
    /// <summary>
    /// This class represents a product in the system. It allows the product to be saved and found in the database.
    /// </summary>
    public class Product : IResource
    {
        /// <summary>
        /// unique identifier for the product. It is created automatically.
        /// </summary>
        public string ProductId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The name given to the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the product is available (1) or not (0).
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// The number of these products currently available for sale.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// A short explanation of what the product is or what it does.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// How much one unit of the product costs.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The identification of the person who first added the product to the system.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The identification of the last person to make changes to the product details.
        /// </summary>
        public string LastUpdatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The exact date and time when the product was added to the system.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The last time the product details were changed.
        /// </summary>
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
