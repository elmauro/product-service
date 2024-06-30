using FluentAssertions;
using MC.Insurance.ApplicationServicesTest.Fixtures;
using MC.ProductService.API.ClientModels;
using MC.ProductService.API.Data;
using MC.ProductService.API.Validators;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace MC.ProductService.Tests.Integration.v1
{
    public class ProductControllerIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private const string ProductRoute = "v1/Product";

        public ProductControllerIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetProductReturnsNoFound()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{ProductRoute}/{productId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetProductReturnsOk()
        {
            // Arrange
            var productToAdd = ProductMockingData.GetProduct();

            // Use the factory to create a scope and resolve the ProductDBContext
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductDBContext>();

            dbContext.Products.Add(productToAdd);
            await dbContext.SaveChangesAsync();

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{ProductRoute}/{productToAdd.ProductId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Should().NotBeNull();

            var actionDataResponse = await response.Content.ReadFromJsonAsync<ActionDataResponse<ProductView>>();
            actionDataResponse.Should().NotBeNull();
            actionDataResponse.Data.Should().NotBeNull();
            actionDataResponse.Data.Name.Should().Be(productToAdd.Name);
            actionDataResponse.Data.Status.Should().Be(productToAdd.Status);
            actionDataResponse.Data.Stock.Should().Be(productToAdd.Stock);
            actionDataResponse.Data.Description.Should().Be(productToAdd.Description);
            actionDataResponse.Data.Price.Should().Be(productToAdd.Price);

            dbContext.Products.Remove(productToAdd);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task AddProductReturnsBadRequest()
        {
            // Arrange
            var productToAdd = ProductMockingData.GetProductRequest();
            productToAdd.Name = string.Empty;

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"{ProductRoute}", productToAdd);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddProductReturnsOk()
        {
            // Arrange
            var productToAdd = ProductMockingData.GetProductRequest();

            // Use the factory to create a scope and resolve the ProductDBContext
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductDBContext>();

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"{ProductRoute}", productToAdd);
            var query = System.Web.HttpUtility.ParseQueryString(response.Headers.Location.Query);
            var productId = query.Get("productId");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Should().NotBeNull();

            var actionDataResponse = await response.Content.ReadFromJsonAsync<ActionDataResponse<ProductRequest>>();
            actionDataResponse.Should().NotBeNull();
            actionDataResponse.Data.Should().NotBeNull();
            actionDataResponse.Data.Name.Should().Be(productToAdd.Name);
            actionDataResponse.Data.Status.Should().Be(productToAdd.Status);
            actionDataResponse.Data.Stock.Should().Be(productToAdd.Stock);
            actionDataResponse.Data.Description.Should().Be(productToAdd.Description);
            actionDataResponse.Data.Price.Should().Be(productToAdd.Price);

            var productToRemove = await dbContext.Products.FindAsync(productId);
            dbContext.Products.Remove(productToRemove);
            await dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateProductReturnsNoFound()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var productToUpdate = ProductMockingData.GetProductRequest();

            var client = _factory.CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"{ProductRoute}/{productId}", productToUpdate);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateProductReturnsBadRequest()
        {
            // Arrange
            var productId = Guid.NewGuid().ToString();
            var productToUpdate = ProductMockingData.GetProductRequest();
            productToUpdate.Name = string.Empty;

            var client = _factory.CreateClient();

            // Act
            var response = await client.PutAsJsonAsync($"{ProductRoute}/{productId}", productToUpdate);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task UpdateProductReturnsOk()
        {
            // Arrange
            var productToUpdate = ProductMockingData.GetProductRequest();

            // Use the factory to create a scope and resolve the ProductDBContext
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ProductDBContext>();

            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync($"{ProductRoute}", productToUpdate);
            var query = System.Web.HttpUtility.ParseQueryString(response.Headers.Location.Query);
            var productId = query.Get("productId");

            var updateResponse = await client.PutAsJsonAsync($"{ProductRoute}/{productId}", productToUpdate);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
            updateResponse.Should().NotBeNull();

            var existingProduct = await dbContext.Products.FindAsync(productId);

            existingProduct.ProductId.Should().Be(productId);
            existingProduct.Name.Should().Be(productToUpdate.Name);
            existingProduct.Status.Should().Be(productToUpdate.Status);
            existingProduct.Stock.Should().Be(productToUpdate.Stock);
            existingProduct.Description.Should().Be(productToUpdate.Description);
            existingProduct.Price.Should().Be(productToUpdate.Price);


            dbContext.Products.Remove(existingProduct);
            await dbContext.SaveChangesAsync();
        }
    }
}
