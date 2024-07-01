﻿using Microsoft.Extensions.Caching.Memory;

namespace MC.ProductService.API.Infrastructure
{
    /// <summary>
    /// Defines methods for caching status information.
    /// </summary>
    public interface IStatusCacheService
    {
        /// <summary>
        /// Retrieves the status name corresponding to the provided status key.
        /// </summary>
        /// <param name="statusKey">The key representing the status.</param>
        /// <returns>The name of the status corresponding to the provided key.</returns>

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
        /// Initializes a new instance of the <see cref="StatusCacheService"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache instance to be used for caching.</param>
        /// <param name="logger">The logger instance for logging.</param>
        public StatusCacheService(IMemoryCache memoryCache, ILogger<StatusCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public string GetStatusName(int statusKey)
        {
            if (!_memoryCache.TryGetValue(CacheKey, out Dictionary<int, string>? statusDictionary))
            {
                _logger.LogInformation("Cache miss for status dictionary. Regenerating...");

                // The cache entry has expired or doesn't exist, so generate a new dictionary
                statusDictionary = GetStatusDictionary();

                // Set cache entry options: 5 minutes sliding expiration
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                // Save the dictionary in the cache
                _memoryCache.Set(CacheKey, statusDictionary, cacheEntryOptions);
            }

            if (statusDictionary != null && statusDictionary.TryGetValue(statusKey, out string? statusName))
            {
                return statusName;
            }
            else
            {
                _logger.LogWarning("Status key {StatusKey} not found in status dictionary", statusKey);
                return "Unknown";
            }
        }

        /// <summary>
        /// Generates the dictionary containing status key-value pairs.
        /// </summary>
        /// <returns>A dictionary with status keys and their corresponding status names.</returns>
        private Dictionary<int, string> GetStatusDictionary()
        {
            // Define the dictionary of status values
            return new Dictionary<int, string>
            {
                { 1, "Active" },
                { 0, "Inactive" }
            };
        }
    }
}
