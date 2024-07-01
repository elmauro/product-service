namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// Represents a mock response for a product with its discount information.
    /// </summary>
    public class MockProductResponse
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        /// <value>The identifier of the product.</value>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the discount applied to the product.
        /// </summary>
        /// <value>The discount information of the product.</value>
        public string Discount { get; set; } = string.Empty;
    }
}
