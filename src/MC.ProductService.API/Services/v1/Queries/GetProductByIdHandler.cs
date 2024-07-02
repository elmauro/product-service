﻿using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Repositories;
using MC.ProductService.API.Infrastructure;
using MC.ProductService.API.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services.v1.Queries
{
    /// <summary>
    /// Handles the retrieval of product details by product ID.
    /// </summary>
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, IActionResult>
    {
        private readonly IProductRepository _repository;
        private readonly IHttpClientMockApi _httpClientMockApi;
        private readonly IStatusCacheService _statusCacheService;
        private readonly ILogger<GetProductByIdHandler> _logger;

        private readonly string _internalServerErrorMessage = "Something went wrong, please try again later.";

        /// <summary>
        /// New instance of the <see cref="GetProductByIdHandler"/>.
        /// </summary>
        /// <param name="repository">The product repository for data access.</param>
        /// <param name="httpClientMockApi">API client for external HTTP calls, such as fetching discounts.</param>
        /// <param name="statusCacheService">Service for caching and retrieving product status.</param>
        /// <param name="logger">Logger for capturing runtime logs and errors.</param>
        public GetProductByIdHandler(
            IProductRepository repository,
            IHttpClientMockApi httpClientMockApi,
            IStatusCacheService statusCacheService,
            ILogger<GetProductByIdHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpClientMockApi = httpClientMockApi ?? throw new ArgumentNullException(nameof(httpClientMockApi));
            _statusCacheService = statusCacheService ?? throw new ArgumentNullException(nameof(statusCacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generates an ObjectResult for internal server errors.
        /// </summary>
        /// <returns>An ObjectResult configured for HTTP 500 Internal Server Error.</returns>
        private ObjectResult GetErrorObjectResult()
        {
            return new ObjectResult((object)_internalServerErrorMessage)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        /// <summary>
        /// Handles the process of retrieving a product identified by ID.
        /// </summary>
        /// <param name="request">The ID of the product to retrieve.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation if needed.</param>
        /// <returns>The result contains the <see cref="ProductView"/> 
        /// corresponding to the specified product ID. Returns null if no product is found with the provided ID.
        /// </returns>
        public async Task<IActionResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Attempt to retrieve a product by its ID asynchronously.
                var product = await _repository.GetProductViewByIdAsync(request.ProductId.ToString());

                if (product == null)
                    return new NotFoundResult();

                var statusName = _statusCacheService.GetStatusName(product.Status);

                // Apply a discount
                var (isSuccess, successResult) = await _httpClientMockApi.GetProductDiscountAsync();

                if (isSuccess && successResult != null)
                {
                    product.StatusName = statusName;
                    product.Discount = int.Parse(successResult[0].Discount); //Call discount service

                    // Calculate the final price after applying the discount.
                    product.FinalPrice = product.Price * (100 - product.Discount) / 100;

                    // Return the updated product object with the discount applied.
                    return new OkObjectResult(new ActionDataResponse<ProductView>(product));
                }
                else
                {
                    _logger.LogError("Failed to retrieve product discount");
                    return GetErrorObjectResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product by ID");
                return GetErrorObjectResult();
            }
        }
    }
}