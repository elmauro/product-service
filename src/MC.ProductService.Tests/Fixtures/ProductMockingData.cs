using AutoFixture;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Models;

namespace MC.Insurance.ApplicationServicesTest.Fixtures
{
    /// <summary>
    /// Provides mocked data instances for testing purposes within the insurance product services.
    /// Utilizes AutoFixture to generate random, valid data for test consistency and isolation.
    /// </summary>
    public static class ProductMockingData
    {
        public static Fixture fixture = new Fixture();

        /// <summary>
        /// Generates a ProductRequest object with randomized properties.
        /// Status is an integer that is randomized to either 0 or 1, ensuring varied test scenarios.
        /// </summary>
        /// <returns>A ProductRequest object with random values, particularly Status set to 0 or 1.</returns>
        public static ProductRequest GetProductRequest()
        {
            var status = fixture.Create<int>() % 2;

            return fixture.Build<ProductRequest>()
                .With(pr => pr.Status, status)
                .Create();
        }

        /// <summary>
        /// Creates a Product data model instance with randomized properties.
        /// ProductId is a new unique GUID for each call.
        /// Timestamps for creation and last update are set to the current UTC time
        /// </summary>
        /// <returns>A Product object with unique GUID and current UTC timestamps for CreatedAt and LastUpdatedAt.</returns>
        public static Product GetProduct() {
			return fixture.Build<Product>()
                .With(p => p.ProductId, Guid.NewGuid().ToString())
                .With(p => p.CreatedAt, DateTime.UtcNow)
                .With(p => p.LastUpdatedAt, DateTime.UtcNow)
                .Create();
		}
    }
}
