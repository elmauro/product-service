using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;

namespace MC.ProductService.API.Infrastructure
{
    /// <summary>
    /// Defines methods for caching status information.
    /// </summary>
    public interface IStatusCacheService
    {
        /// <summary>
        /// Gets the name of a status based on a status key.
        /// </summary>
        /// <param name="statusKey">The number that represents a specific status.</param>
        /// <returns>The name of the status linked to the given key.</returns>
        string GetStatusName(int statusKey);
    }

    /// <summary>
    /// Provides caching functionality for status information.
    /// </summary>
    public class StatusCacheService : IStatusCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private const string CacheKey = "StatusDictionary";
        private readonly ILogger<StatusCacheService> _logger;

        /// <summary>
        /// Creates a new service to manage cached status information.
        /// </summary>
        /// <param name="memoryCache">The memory cache instance to be used for caching.</param>
        /// <param name="logger">The logger instance for logging.</param>
        public StatusCacheService(IMemoryCache memoryCache, ILogger<StatusCacheService> logger)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string GetStatusName(int statusKey)
        {
            if (!_memoryCache.TryGetValue(CacheKey, out Dictionary<int, string>? statusDictionary))
            {
                _logger.LogInformation("No saved data for status. Making new...");

                // If the status information isn't saved, create a new list of statuses
                statusDictionary = GetStatusDictionary();

                // Settings for how long to save the data: renews every 5 minutes
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                // Save the new list in the memory cache
                _memoryCache.Set(CacheKey, statusDictionary, cacheEntryOptions);
            }

            // Try to get the status name using the key; if it's not there, log a warning and return "Unknown"
            if (statusDictionary != null && statusDictionary.TryGetValue(statusKey, out string? statusName))
            {
                return statusName;
            }
            else
            {
                _logger.LogWarning("Could not find the status key {StatusKey} in the list", statusKey);
                return "Unknown";
            }
        }

        /// <summary>
        /// Creates a list of statuses with their names.
        /// </summary>
        /// <returns>A list showing each status number and what it means.</returns>
        private Dictionary<int, string> GetStatusDictionary()
        {
            // A simple list of status numbers and their meanings
            return new Dictionary<int, string>
            {
                { 1, "Active" },
                { 0, "Inactive" }
            };
        }
    }
}