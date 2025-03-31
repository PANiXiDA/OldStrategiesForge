using System.Net;
using Newtonsoft.Json;

namespace Common.Converters;

public class IPEndPointNewtonsoftConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IPEndPoint);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        IPEndPoint endpoint = (IPEndPoint)value;
        writer.WriteValue($"{endpoint.Address}:{endpoint.Port}");
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        string endpointString = reader.Value as string;
        if (string.IsNullOrWhiteSpace(endpointString))
        {
            throw new JsonSerializationException("Строка адреса не может быть пустой.");
        }

        var parts = endpointString.Split(':');
        if (parts.Length != 2)
        {
            throw new JsonSerializationException("Неверный формат адреса. Ожидается формат 'IP:Port'.");
        }

        if (!IPAddress.TryParse(parts[0], out var ip))
        {
            throw new JsonSerializationException("Неверный IP-адрес.");
        }

        if (!int.TryParse(parts[1], out var port))
        {
            throw new JsonSerializationException("Неверный порт.");
        }

        return new IPEndPoint(ip, port);
    }
}
