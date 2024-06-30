using MC.ProductService.API.ClientModels;
using System.Text.Json;

namespace MC.ProductService.API.Infrastructure
{
    /// <summary>
    /// Defines methods for interacting with a mock API to get product discount information.
    /// </summary>
    public interface IHttpClientMockApi
    {
        /// <summary>
        /// Asynchronously gets the product discount information.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a success flag and the list of product responses.</returns>
        public Task<(bool IsSuccess, List<MockProductResponse> SuccessResult)> GetProductDiscountAsync();
    }

    /// <summary>
    /// Provides implementation for interacting with a mock API to get product discount information.
    /// </summary>
    public class HttpClientMockApiService : IHttpClientMockApi
    {
        private readonly HttpClient _client;

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private const string ProductRoute = "https://6680a0be56c2c76b495c7127.mockapi.io/v1/product";

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientMockApiService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to make requests.</param>
        /// <exception cref="ArgumentNullException">Thrown when the logger is null.</exception>

        public HttpClientMockApiService(
            HttpClient httpClient)
        {
            _client = httpClient;
        }

        public async Task<(bool IsSuccess, List<MockProductResponse> SuccessResult)> GetProductDiscountAsync()
        {
            return await ExecuteAsync<List<MockProductResponse>>(async () =>
                await _client.GetAsync(ProductRoute)
            );
        }

        /// <summary>
        /// Executes an HTTP request and processes the response.
        /// </summary>
        /// <typeparam name="T">The type of the success result.</typeparam>
        /// <param name="request">The function that performs the HTTP request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with a success flag and the success result.</returns>
        private async Task<(bool IsSuccess, T SuccessResult)> ExecuteAsync<T>(Func<Task<HttpResponseMessage>> request)
        {
            var result = (IsSuccess: false, SuccessResult: default(T));
            var httpResponse = await request();

            result.IsSuccess = true;

            // De-serialize using our json settings
            result.SuccessResult = await httpResponse.Content
                .ReadFromJsonAsync<T>(jsonOptions);

            return result;
        }
    }
}
