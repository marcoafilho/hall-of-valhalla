using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HallOfValhalla.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace HallOfValhalla.Services
{
    public class VenueService : IVenueService
    {
        private const string CacheKey = "VenueService.GetVenuesAsync";
        private const string BaseUrl = "https://api.openbrewerydb.org/breweries/";

        private static JsonSerializerOptions _serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        private readonly IMemoryCache _memoryCache;

        public VenueService(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<List<Venue>> GetVenuesAsync()
        {
            // TODO: Handle exceptions.
            return await _memoryCache.GetOrCreateAsync<List<Venue>>(CacheKey, GetVenuesFactory);
        }

        private async Task<List<Venue>> GetVenuesFactory(ICacheEntry cacheEntry)
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return await _httpClient.GetFromJsonAsync<List<Venue>>(BaseUrl, _serializerOptions);
        }
    }
}
