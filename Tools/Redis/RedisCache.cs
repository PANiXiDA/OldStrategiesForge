using System.Text.Json;
using StackExchange.Redis;

namespace Tools.Redis;

public class RedisCache : IRedisCache
{
    private readonly IDatabase _database;

    public RedisCache(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty || value == RedisValue.Null)
        {
            throw new KeyNotFoundException($"Key '{key}' was not found in Redis.");
        }

        return JsonSerializer.Deserialize<T>(value.ToString()!)!;
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
