namespace MC.ProductService.API.ClientModels
{
    /// <summary>
    /// Represents a simplified or fake response for a product, including any discount details.
    /// This class is typically used for testing or mock-up purposes to simulate how real data would be handled.
    /// </summary>
    public class MockProductResponse
    {
        /// <summary>
        /// The unique identifier for the product.
        /// </summary>
        /// <value>The identifier of the product.</value>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Details about any discount applied to the product.
        /// </summary>
        /// <value>The discount information of the product.</value>
        public string Discount { get; set; } = string.Empty;
    }
}
