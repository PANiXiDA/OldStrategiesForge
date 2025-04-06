using Common.Helpers;
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
        var serializedValue = JsonHelper.Serialize(value);
        await _database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty || value == RedisValue.Null)
        {
            throw new KeyNotFoundException($"Key '{key}' was not found in Redis.");
        }

        if (JsonHelper.TryDeserialize<T>(value.ToString()!, out var result))
        {
            return result!;
        }
        else
        {
            throw new Exception("Deserialization failed.");
        }
    }

    public async Task<(bool Found, T? Value)> TryGetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        if (value.IsNullOrEmpty || value == RedisValue.Null)
        {
            return (false, default);
        }

        return (JsonHelper.TryDeserialize<T>(value.ToString()!, out var result), result);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
