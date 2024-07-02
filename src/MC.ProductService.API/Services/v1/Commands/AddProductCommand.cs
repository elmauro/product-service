using MC.ProductService.API.ClientModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services.v1.Commands
{
    /// <summary>
    /// Command to add a new product using MediatR.
    /// </summary>
    public class AddProductCommand : IRequest<IActionResult>
    {
        /// <summary>
        /// Product request containing all necessary data to create a new product.
        /// </summary>
        /// <value>
        /// The product request details needed for product creation.
        /// </value>
        public ProductRequest Product { get; set; }

        /// <summary>
        /// New instance of the <see cref="AddProductCommand"/> class.
        /// </summary>
        /// <param name="product">
        /// The <see cref="ProductRequest"/> containing the details of the product to add.
        /// </param>
        public AddProductCommand(ProductRequest product)
        {
            Product = product;
        }
    }
}
