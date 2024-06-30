using AutoFixture;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data.Models;

namespace MC.Insurance.ApplicationServicesTest.Fixtures
{
	public static class ProductMock
	{
        public static Fixture fixture = new Fixture();

        public static ProductRequest GetProductRequest()
        {
            var status = fixture.Create<int>() % 2;

            return fixture.Build<ProductRequest>()
                .With(pr => pr.Status, status)
                .Create();
        }

        public static Product GetProduct() {
			return fixture.Build<Product>()
                .With(p => p.ProductId, Guid.NewGuid().ToString())
                .With(p => p.CreatedAt, DateTime.UtcNow)
                .With(p => p.LastUpdatedAt, DateTime.UtcNow)
                .Create();
		}
    }
}
