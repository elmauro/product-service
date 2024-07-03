using MC.ProductService.API.Data.Models;

namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// This class holds information about a product that someone wants to add or change in the system.
    /// </summary>
    public class ProductRequest
    {
        /// <see cref="Product.Name"/>
        public string Name { get; set; } = string.Empty;

        /// <see cref="Product.Status"/>
        public int Status{ get; set; }

        /// <see cref="Product.Stock"/>
        public int Stock { get; set; }

        /// <see cref="Product.Description"/>
        public string Description { get; set; } = string.Empty;

        /// <see cref="Product.Price"/>
        public int Price { get; set; }
    }
}
