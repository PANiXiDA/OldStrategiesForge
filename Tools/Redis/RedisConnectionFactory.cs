using StackExchange.Redis;

namespace Tools.Redis;

public static class RedisConnectionFactory
{
    private static Lazy<IConnectionMultiplexer> _connection = null!;

    public static void Initialize(string configuration)
    {
        _connection = new Lazy<IConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(configuration));
    }

    public static IConnectionMultiplexer GetConnection()
    {
        if (_connection == null)
        {
            throw new InvalidOperationException("Redis connection is not initialized. Call Initialize first.");
        }

        return _connection.Value;
    }
}
