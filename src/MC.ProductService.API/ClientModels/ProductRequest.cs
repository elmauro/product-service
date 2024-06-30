using MC.ProductService.API.Data.Models;

namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// Represents a request model for capturing product-related data from clients or for internal data handling.
    /// This class is typically used for creating or updating product records.
    /// </summary>
    public class ProductRequest
    {
        /// <see cref="Product.Name"/>
        public string Name { get; set; }

        /// <see cref="Product.Status"/>
        public int Status{ get; set; }

        /// <see cref="Product.Stock"/>
        public int Stock { get; set; }

        /// <see cref="Product.Description"/>
        public string Description { get; set; }

        /// <see cref="Product.Price"/>
        public int Price { get; set; }
    }
}
