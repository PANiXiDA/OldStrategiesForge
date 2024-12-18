using Tools.Redis;

namespace APIGateway.Extensions.Cooldowns;

public static class Cooldowns
{
    public static async Task<bool> CanPerformActionAsync(string actionKey, IRedisCache cache, TimeSpan cooldownDuration)
    {
        var cacheKey = $"Cooldown:{actionKey}";

        if (!await cache.ExistsAsync(cacheKey))
        {
            await cache.SetAsync(cacheKey, DateTime.UtcNow, cooldownDuration);
            return true;
        }

        var lastActionTime = await cache.GetAsync<DateTime?>(cacheKey);

        if (lastActionTime.HasValue && (DateTime.UtcNow - lastActionTime.Value) < cooldownDuration)
        {
            return false;
        }

        await cache.SetAsync(cacheKey, DateTime.UtcNow, cooldownDuration);
        return true;
    }

    public static async Task<bool> CanSendRecoveryEmailAsync(string email, IRedisCache cache)
    {
        var actionKey = $"RecoveryEmail:{email}";
        return await CanPerformActionAsync(actionKey, cache, TimeSpan.FromMinutes(15));
    }

    public static async Task<bool> CanSendConfirmAccountAsync(string email, IRedisCache cache)
    {
        var actionKey = $"ConfirmAccount:{email}";
        return await CanPerformActionAsync(actionKey, cache, TimeSpan.FromMinutes(15));
    }
}
