using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HallOfValhalla.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace HallOfValhalla.Services
{
    public class TopicService : ITopicService
    {
        private const string CacheKey = "TopicService.GetTopicsAsync";
        private const string BaseUrl = "https://gateway.marvel.com/v1/public";

        private static JsonSerializerOptions _serializerOptions = new() {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;

        private readonly IMemoryCache _memoryCache;

        private readonly string _apiPrivateKey;
        private readonly string _apiPublicKey;

        public TopicService(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClientFactory.CreateClient();
            _apiPrivateKey = configuration.GetSection("Marvel:PrivateKey").Value;
            _apiPublicKey = configuration.GetSection("Marvel:PublicKey").Value;
        }

        public async Task<List<Topic>> GetTopicsAsync()
        {
            // TODO: Handle exceptions.
            return await _memoryCache.GetOrCreateAsync<List<Topic>>(CacheKey, GetTopicsFactory);
        }

        private async Task<List<Topic>> GetTopicsFactory(ICacheEntry cacheEntry)
        {
            UriBuilder baseUri = new UriBuilder(BaseUrl + "/characters");
            long timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
            baseUri.Query = $"ts={timeStamp}&apikey={_apiPublicKey}&hash={Hash(timeStamp)}";

            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            Topics topics = await _httpClient.GetFromJsonAsync<Topics>(baseUri.ToString(), _serializerOptions);

            return topics.Data.Topics;
        }

        private string Hash(long timeStamp)
        {
            return Helpers.Digest.MD5.Hexdigest($"{timeStamp}{_apiPrivateKey}{_apiPublicKey}");
        }
    }
}
