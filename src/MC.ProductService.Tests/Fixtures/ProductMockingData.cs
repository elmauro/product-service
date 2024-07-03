using AutoFixture;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Models;

namespace MC.Insurance.ApplicationServicesTest.Fixtures
{
    /// <summary>
    /// Provides fake data for testing the insurance product services.
    /// Uses AutoFixture to automatically create realistic and random data for thorough and independent tests.
    /// </summary>
    public static class ProductMockingData
    {
        public static Fixture fixture = new Fixture();

        /// <summary>
        /// Creates a new ProductRequest object with random values.
        /// The 'Status' property is either 0 or 1, chosen randomly, to test different scenarios.
        /// </summary>
        /// <returns>A new ProductRequest with random values, including a 'Status' that is either 0 or 1.</returns>
        public static ProductRequest GetProductRequest()
        {
            var status = fixture.Create<int>() % 2;

            return fixture.Build<ProductRequest>()
                .With(pr => pr.Status, status)
                .Create();
        }

        /// <summary>
        /// Generates a new Product object each time it's called, with unique identifiers and timestamps.
        /// The 'ProductId' is a unique GUID, and the 'CreatedAt' and 'LastUpdatedAt' timestamps are set to the current time.
        /// </summary>
        /// <returns>A new Product with a unique ID and current timestamps for when it was created and last updated.</returns>
        public static Product GetProduct() {
			return fixture.Build<Product>()
                .With(p => p.ProductId, Guid.NewGuid().ToString())
                .With(p => p.CreatedAt, DateTime.UtcNow)
                .With(p => p.LastUpdatedAt, DateTime.UtcNow)
                .Create();
		}
    }
}
