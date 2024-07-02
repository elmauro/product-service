using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services.v1.Queries
{
    /// <summary>
    /// Represents a query to retrieve a product by its unique identifier.
    /// </summary>
    public class GetProductByIdQuery : IRequest<IActionResult>
    {
        /// <summary>
        /// The unique identifier for the product.
        /// </summary>
        /// <value>
        /// The GUID representing the unique identifier of the product to be retrieved.
        /// </value>
        public Guid ProductId { get; set; }

        /// <summary>
        /// New instance of the <see cref="GetProductByIdQuery"/> class with the specified product identifier.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to retrieve.</param>
        public GetProductByIdQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}
