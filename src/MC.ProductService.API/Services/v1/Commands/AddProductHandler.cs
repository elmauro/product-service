using AutoMapper;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Controllers.v1;
using MC.ProductService.API.Data.Models;
using MC.ProductService.API.Data.Repositories;
using MC.ProductService.API.Infrastructure;
using MC.ProductService.API.Options;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MC.ProductService.API.Services.v1.Commands
{
    /// <summary>
    /// Handles the addition of new products by processing <see cref="AddProductCommand"/>.
    /// </summary>
    public class AddProductHandler : IRequestHandler<AddProductCommand, IActionResult>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHttpClientMockApi _httpClientMockApi;
        private readonly IStatusCacheService _statusCacheService;
        private readonly ILogger<AddProductHandler> _logger;

        private readonly string _internalServerErrorMessage = "Something went wrong, please try again later.";
        private const string systemUser = "system";

        /// <summary>
        /// Initializes a new instance of the <see cref="AddProductHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository to interact with the data layer.</param>
        /// <param name="mapper">The mapper to transform data models.</param>
        /// <param name="httpClientMockApi">A mock API client for external HTTP calls.</param>
        /// <param name="statusCacheService">Service for caching status information.</param>
        /// <param name="logger">Logger for logging runtime information and errors.</param>
        /// <exception cref="ArgumentNullException">Thrown when an injected dependency is null.</exception>
        public AddProductHandler(
            IProductRepository repository,
            IMapper mapper,
            IHttpClientMockApi httpClientMockApi,
            IStatusCacheService statusCacheService,
            ILogger<AddProductHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpClientMockApi = httpClientMockApi ?? throw new ArgumentNullException(nameof(httpClientMockApi));
            _statusCacheService = statusCacheService ?? throw new ArgumentNullException(nameof(statusCacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Generates an ObjectResult for internal server errors.
        /// </summary>
        /// <returns>An ObjectResult configured for internal server errors.</returns>
        private ObjectResult GetErrorObjectResult()
        {
            return new ObjectResult((object)_internalServerErrorMessage)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        /// <summary>
        /// Handles the addition of a product to the database.
        /// </summary>
        /// <param name="request">The command containing the product data.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The result of the Add product operation.</returns>
        public async Task<IActionResult> Handle(AddProductCommand request, CancellationToken cancellationToken)
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
