using MC.ProductService.API.ClientModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services.v1.Commands
{
    /// <summary>
    /// Command to update an existing product in the system.
    /// </summary>
    public class UpdateProductCommand : IRequest<IActionResult>
    {
        /// <summary>
        /// Unique identifier of the product to be updated.
        /// </summary>
        /// <value>
        /// The ID of the product.
        /// </value>
        public Guid ProductId { get; }

        /// <summary>
        /// Product details to be applied during the update.
        /// </summary>
        /// <value>
        /// The product data to update, encapsulated in a <see cref="ProductRequest"/>.
        /// </value>
        public ProductRequest Product { get; }

        /// <summary>
        /// New instance of the <see cref="UpdateProductCommand"/> class.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to update.</param>
        /// <param name="product">The new details to apply to the product.</param>
        public UpdateProductCommand(Guid productId, ProductRequest product)
        {
            ProductId = productId;
            Product = product;
        }
    }
}
