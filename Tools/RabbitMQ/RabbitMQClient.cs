using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Tools.RabbitMQ;

internal class RabbitMQClient : IRabbitMQClient, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _hostname;
    private readonly ILogger<RabbitMQClient> _logger;

    public RabbitMQClient(string hostname, ILogger<RabbitMQClient> logger)
    {
        var factory = new ConnectionFactory() { HostName = hostname };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _hostname = hostname;
        _logger = logger;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }

    public void SendMessage<T>(T messageObject, string queue)
    {
        try
        {
            _channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var jsonMessage = JsonConvert.SerializeObject(messageObject);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel.BasicPublish(exchange: "",
                                 routingKey: queue,
                                 basicProperties: null,
                                 body: body);

            _logger.LogInformation($"[x] Sent JSON message to queue '{queue}': {jsonMessage}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending JSON message to queue '{queue}': {messageObject}");
        }
    }

    public void StartReceiving<T>(Action<T> handleMessage, string queue)
    {
        try
        {
            _channel.QueueDeclare(queue: queue,
                                     durable: false,
                                     exclusive: false,  
                                     autoDelete: false,
                                     arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"[x] Received JSON message from queue '{queue}': {message}");

                try
                {
                    var messageObject = JsonConvert.DeserializeObject<T>(message);
                    if (messageObject != null)
                    {
                        handleMessage(messageObject);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Error deserializing JSON message");
                }
            };

            _channel.BasicConsume(queue: queue,
                                     autoAck: false,
                                     consumer: consumer);

            _logger.LogInformation($"Waiting for messages in queue '{queue}'...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error receiving JSON messages from queue '{queue}'");
        }
    }
}
