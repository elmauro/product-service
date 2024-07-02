using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Options;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using MC.ProductService.API.Services.v1.Queries;
using MC.ProductService.API.Services.v1.Commands;
namespace MC.ProductService.API.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the product information by ID.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <response code="200">Returns the product information.</response> 
        /// <response code="404">The product information was not found.</response>
        /// <returns>The product information if found; otherwise, a 404 status code.</returns>
        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(IActionDataResponse<ProductView>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid productId)
        {
            return await _mediator.Send(new GetProductByIdQuery(productId));
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product information to add.</param>
        /// <response code="201">The product was successfully created.</response> 
        /// <response code="400">The product information was not valid.</response>
        /// <returns>The created product information.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(IActionDataResponse<ProductRequest>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductRequest product)
        {
            return await _mediator.Send(new AddProductCommand(product));
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to update.</param>
        /// <param name="product">The product information to update.</param>
        /// <response code="204">The product was successfully updated.</response>
        /// <response code="400">The product information was not valid.</response>
        /// <response code="404">The product information was not found.</response>
        /// <returns>No content if the update is successful; otherwise, an error status code.</returns>
        [HttpPut("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse),  StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid productId, [FromBody] ProductRequest product)
        {
            return await _mediator.Send(new UpdateProductCommand(productId, product));
        }
    }
}
