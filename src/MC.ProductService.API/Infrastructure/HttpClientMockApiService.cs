using MC.ProductService.API.ClientModels;
using System.Text.Json;

namespace MC.ProductService.API.Infrastructure
{
    /// <summary>
    /// This interface provides methods to get discount information about products from a fake API.
    /// </summary>
    public interface IHttpClientMockApi
    {
        /// <summary>
        /// Asks the fake API for discount information about products.
        /// </summary>
        /// <returns>A result that says whether the request was successful and gives a list of product discounts if it was.</returns>
        Task<(bool IsSuccess, List<MockProductResponse>? SuccessResult)> GetProductDiscountAsync();
    }

    /// <summary>
    /// This class handles getting discount information about products from a fake API.
    /// </summary>
    public class HttpClientMockApiService : IHttpClientMockApi
    {
        private readonly HttpClient _client;
        private readonly ILogger<HttpClientMockApiService> _logger;

        private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private const string ProductRoute = "https://6680a0be56c2c76b495c7127.mockapi.io/v1/product";

        /// <summary>
        /// Sets up the service with an HTTP client and a logger.
        /// </summary>
        /// <param name="httpClient">The client that will send requests to the API.</param>
        /// <param name="logger">A tool that records what happens during the operations.</param>
        public HttpClientMockApiService(
            HttpClient httpClient,
            ILogger<HttpClientMockApiService> logger)
        {
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<(bool IsSuccess, List<MockProductResponse>? SuccessResult)> GetProductDiscountAsync()
        {
            return await ExecuteAsync<List<MockProductResponse>>(async () =>
                await _client.GetAsync(ProductRoute)
            );
        }

        /// <summary>
        /// Runs an HTTP request and handles the response.
        /// </summary>
        /// <typeparam name="T">The type of data we expect if the request succeeds.</typeparam>
        /// <param name="request">The action that makes the HTTP request.</param>
        /// <returns>A result that includes whether the request succeeded and the data if it did.</returns>
        private async Task<(bool IsSuccess, T? SuccessResult)> ExecuteAsync<T>(Func<Task<HttpResponseMessage>> request)
        {
            var result = (IsSuccess: false, SuccessResult: default(T));

            try
            {
                var httpResponse = await request();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogWarning("HTTP request failed with status code {StatusCode}", httpResponse.StatusCode);
                    return result;
                }

                result.IsSuccess = true;

                // De-serialize using our json settings
                result.SuccessResult = await httpResponse.Content
                    .ReadFromJsonAsync<T>(jsonOptions);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                return result;
            }
        }
    }
}
