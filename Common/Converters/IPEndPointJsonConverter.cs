using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Common.Converters;

public class IPEndPointJsonConverter : JsonConverter<IPEndPoint>
{
    public override IPEndPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? endpointString = reader.GetString();
        if (string.IsNullOrWhiteSpace(endpointString))
        {
            throw new JsonException("Строка адреса не может быть пустой.");
        }

        var parts = endpointString.Split(':');
        if (parts.Length != 2)
        {
            throw new JsonException("Неверный формат адреса. Ожидается формат 'IP:Port'.");
        }

        if (!IPAddress.TryParse(parts[0], out var ip))
        {
            throw new JsonException("Неверный IP-адрес.");
        }

        if (!int.TryParse(parts[1], out var port))
        {
            throw new JsonException("Неверный порт.");
        }

        return new IPEndPoint(ip, port);
    }

    public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
    {
        string endpointString = $"{value.Address}:{value.Port}";
        writer.WriteStringValue(endpointString);
    }
}
