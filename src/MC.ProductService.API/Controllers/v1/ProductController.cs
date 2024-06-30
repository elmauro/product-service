using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Services.v1;
using MC.ProductService.API.Validators;
using Microsoft.AspNetCore.Mvc;
namespace MC.ProductService.API.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Allows to get the Product Information.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="200">Returns the product information.</response> 
        /// <response code="404">The product information was not found.</response>
        /// <returns></returns>
        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(IActionDataResponse<ProductView>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string productId)
        {
            return await _productService.GetProductByIdAsync(productId);
        }

        /// <summary>
        /// Allows to add a new Product Information.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="201">Returns the product information.</response> 
        /// <response code="400">The product information was not valid data.</response>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(IActionDataResponse<ProductRequest>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductRequest product)
        {
            return await _productService.AddProductAsync(product);
        }

        /// <summary>
        /// Allows to update thre Product Information.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <response code="204">Returns no content.</response>
        /// <response code="400">The product information was not valid data.</response>
        /// <response code="404">The product information was not found.</response>
        /// <returns></returns>
        [HttpPut("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] Guid productId, [FromBody] ProductRequest product)
        {
            return await _productService.UpdateProductAsync(productId, product);
        }
    }
}
