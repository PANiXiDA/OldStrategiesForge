namespace Tools.Redis;

public interface IRedisCache
{
    Task<bool> ExistsAsync(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task<T> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}

