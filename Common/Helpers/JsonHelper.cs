using Common.Converters;
using System.Text.Json;

namespace Common.Helpers;

public static class JsonHelper
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    static JsonHelper()
    {
        _options.Converters.Add(new IPEndPointJsonConverter());
    }

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

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, _options);
    }
}
