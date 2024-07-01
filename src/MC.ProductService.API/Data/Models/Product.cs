using MC.ProductService.API.Data.ResourceConfiguration;

namespace MC.ProductService.API.Data.Models
{
    /// <summary>
    /// Product entity in the system. This class implements the <see cref="IResource"/> interface,
    /// indicating it is a resource that can be managed and persisted within the system.
    /// </summary>
    public class Product : IResource
    {
        /// <summary>
        /// The unique identifier for the product. By default, it is initialized to a new GUID string.
        /// </summary>
        public string ProductId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// The name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The status of the product, indicating if is Active or Not.
        /// </summary>
        public int Status { get; set; } = 0;

        /// <summary>
        /// The stock level of the product, representing how many units are available.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// A description of the product.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The price of the product. This is the cost for one unit.
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// The identifier for the user who created the product.
        /// </summary>
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The identifier for the last user who updated the product.
        /// </summary>
        public string LastUpdatedBy { get; set; } = string.Empty;

        /// <summary>
        /// The date and time when the product was initially created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the product was last updated.
        /// </summary>
        public DateTimeOffset LastUpdatedAt { get; set; }
    }
}
