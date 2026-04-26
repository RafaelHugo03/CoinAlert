using System.Text.Json;
using CoinAlertApi.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace CoinAlertApi.Infrastructure.Cache;

public class CacheService(IDistributedCache cache) : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(10);

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await cache.GetStringAsync(key);
        return json is null ? default : JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
        };

        var json = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, json, options);
    }

    public async Task InvalidateAsync(string key)
    {
        await cache.RemoveAsync(key);
    }
}
