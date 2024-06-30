using AutoMapper;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Repositories;
using MC.ProductService.API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using MC.ProductService.API.Options;
using MC.ProductService.API.Controllers.v1;
using MC.ProductService.API.Infrastructure;

namespace MC.ProductService.API.Services.v1
{
    public interface IProductService
    {
        /// <summary>
        /// Asynchronously retrieves a product by its ID.
        /// </summary>
        /// <param name="productId">The unique identifier for the product to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the <see cref="ProductView"/> 
        /// corresponding to the specified product ID. Returns null if no product is found with the provided ID.
        /// </returns>
        Task<IActionResult> GetProductByIdAsync(string productId);

        /// <summary>
        /// Asynchronously adds a new product to the database.
        /// </summary>
        /// <param name="product">The product information used to create a new product entry. This data is encapsulated
        /// in a <see cref="ProductRequest"/> object.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task completes once the product has been successfully
        /// added to the database.
        /// </returns>
        Task<IActionResult> AddProductAsync(ProductRequest product);

        /// <summary>
        /// Asynchronously updates an existing product in the database.
        /// </summary>
        /// <param name="productId">The product ID to update</param>
        /// <param name="product">The product information to update. The product to update is identified by the ID 
        /// within the <see cref="ProductRequest"/> object, and the other details in the object represent the new 
        /// values to be stored.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task completes once the product has been successfully
        /// updated in the database.
        /// </returns>
        Task<IActionResult> UpdateProductAsync(Guid productId, ProductRequest product);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpClientMockApi _httpClientMockApi;
        private readonly IStatusCacheService _statusCacheService;

        private readonly string _internalServerErrorMessage = "Something went wrong, please try again later.";
        private const string systemUser = "system";

        public ProductService(IProductRepository repository, 
            IMapper mapper, 
            IHttpClientMockApi httpClientMockApi,
            IStatusCacheService statusCacheService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpClientMockApi = httpClientMockApi ?? throw new ArgumentNullException(nameof(httpClientMockApi));
            _statusCacheService = statusCacheService ?? throw new ArgumentNullException(nameof(statusCacheService));
        }

        public async Task<IActionResult> GetProductByIdAsync(string productId)
        {
            // Attempt to retrieve a product by its ID asynchronously.
            var product = await _repository.GetProductViewByIdAsync(productId);

            if (product == null)
                return new NotFoundResult();

            var statusName = _statusCacheService.GetStatusName(product.Status);

            // Apply a discount
            var (isSuccess, successResult) = await _httpClientMockApi.GetProductDiscountAsync();

            if (isSuccess)
            {
                product.StatusName = statusName;
                product.Discount = int.Parse(successResult[0].Discount); //Call discount service

                // Calculate the final price after applying the discount.
                product.FinalPrice = product.Price * (100 - product.Discount) / 100;

                // Return the updated product object with the discount applied.
                return new OkObjectResult(new { data = product });
            }
            else
            {
                // Return 500 status code (Internal Server Error)
                var objectResult = new ObjectResult((object)_internalServerErrorMessage);
                objectResult.StatusCode = StatusCodes.Status500InternalServerError;
                return objectResult;
            }
        }

        public async Task<IActionResult> AddProductAsync(ProductRequest product)
        {
            // Map the incoming product DTO to the Product entity model using an object mapper.
            var newProduct = _mapper.Map<Product>(product);

            // Set the 'CreatedBy' property and the 'LastUpdatedBy' property to the current system user, indicating who created the product.
            newProduct.CreatedBy = systemUser;
            newProduct.LastUpdatedBy = systemUser;


            await _repository.AddProductAsync(newProduct);

            var response = new ActionDataResponse<ProductRequest>(product);

            return new CreatedAtActionResult(nameof(ProductController.Create), null, new { productId = newProduct.ProductId }, response);
        }

        public async Task<IActionResult> UpdateProductAsync(Guid productId, ProductRequest product)
        {
            // Fetch the existing product by its ID asynchronously from the repository.
            var existingProduct = await _repository.GetProductByIdAsync(productId.ToString());

            // Map the incoming product DTO to a new Product entity model.
            var newProduct = _mapper.Map<Product>(product);

            if (existingProduct == null)
                return new NotFoundResult();

            // Map the updated values from the newly created Product entity to the existing product entity.
            var productToUpdate = _mapper.Map(newProduct, existingProduct);

            // Update the 'LastUpdatedBy' and 'LastUpdatedAt' properties to reflect the current system user and the current UTC time.
            productToUpdate.CreatedBy = systemUser;
            productToUpdate.LastUpdatedBy = systemUser;
            productToUpdate.LastUpdatedAt = DateTime.UtcNow;

            await _repository.UpdateProductAsync(productToUpdate);
            return new NoContentResult();
        }
    }
}
