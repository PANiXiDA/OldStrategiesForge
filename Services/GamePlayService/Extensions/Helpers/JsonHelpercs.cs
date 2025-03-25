using System.Text.Json;

namespace GamePlayService.Extensions.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static bool TryDeserialize<T>(string json, out T? result)
    {
        result = default;
        if (string.IsNullOrWhiteSpace(json))
        {
            return false;
        }

        try
        {
            result = JsonSerializer.Deserialize<T>(json, _options);
            return result != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}