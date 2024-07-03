using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MC.ProductService.API.Data.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a product view model by its ID.
        /// </summary>
        /// <param name="productId">The unique identifier for the product to retrieve.</param>
        /// <returns>
        /// The result contains the <see cref="ProductView"/>
        /// corresponding to the specified product ID, or null if no product is found.
        /// </returns>
        Task<ProductView?> GetProductViewByIdAsync(string productId);

        /// <summary>
        /// Retrieves a product entity by its ID.
        /// </summary>
        /// <param name="productId">The unique identifier for the product to retrieve.</param>
        /// <returns>
        /// The result contains the <see cref="Product"/>
        /// entity corresponding to the specified product ID, or null if no product is found.
        /// </returns>
        Task<Product?> GetProductByIdAsync(string productId);

        /// <summary>
        /// A new product to the database.
        /// </summary>
        /// <param name="product">The product entity to add.</param>
        /// <returns>
        /// The result contains the <see cref="Product"/> entity that was added to the database.
        /// </returns>
        Task AddProductAsync(Product product);

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product entity to update.</param>
        /// <returns>
        /// </returns>
        Task UpdateProductAsync(Product product);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ProductDBContext _context;
        private const string ProductNotFound = "Product not found.";

        public ProductRepository(ProductDBContext context)
        {
            _context = context;
        }

        public async Task<ProductView?> GetProductViewByIdAsync(string productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Select(ProductView.Project())
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product?> GetProductByIdAsync(string productId)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Attach(product);
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }

}
