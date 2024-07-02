using AutoMapper;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Controllers.v1;
using MC.ProductService.API.Data.Models;
using MC.ProductService.API.Data.Repositories;
using MC.ProductService.API.Options;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services.v1.Commands
{
    /// <summary>
    /// Handles the addition of new products by processing <see cref="AddProductCommand"/>.
    /// </summary>
    public class AddProductHandler : ProductHandlerBase<AddProductCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository to interact with the data layer.</param>
        /// <param name="mapper">The mapper to transform data models.</param>
        /// <param name="logger">Logger for logging runtime information and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when an injected dependency is null.</exception>
        public AddProductHandler(
            IProductRepository repository,
            IMapper mapper,
            ILogger<AddProductHandler> logger) : base(repository, mapper, logger)
        {
        }

        /// <summary>
        /// Handles the addition of a product to the database.
        /// </summary>
        /// <param name="request">The command containing the product data.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The result of the Add product operation.</returns>
        public override async Task<IActionResult> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map the incoming product DTO to the Product entity model.
                var newProduct = _mapper.Map<Product>(request.Product);

                // Set the 'CreatedBy' property and the 'LastUpdatedBy' property to the current system user, indicating who created the product.
                newProduct.CreatedBy = systemUser;
                newProduct.LastUpdatedBy = systemUser;


                await _repository.AddProductAsync(newProduct);

                var response = new ActionDataResponse<ProductRequest>(request.Product);

                return new CreatedAtActionResult(nameof(ProductController.Create), null, new { productId = newProduct.ProductId }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new product");
                return GetErrorObjectResult();
            }
        }
    }
}
