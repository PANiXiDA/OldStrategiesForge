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

    public void StartReceiving<T>(Func<T, Task> handleMessage, string queue)
    {
        StartReceiving<T>((message, _, _) => handleMessage(message), queue);
    }

    public void StartReceiving<T>(Func<T, IBasicProperties, IModel, Task> handleMessage, string queue)
    {
        try
        {
            _channel.QueueDeclare(queue: queue,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation($"[x] Received JSON message from queue '{queue}': {message}");

                try
                {
                    var messageObject = JsonConvert.DeserializeObject<T>(message);
                    if (messageObject != null)
                    {
                        await handleMessage(messageObject, ea.BasicProperties, _channel);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (JsonException jsonEx)
                {
                    _logger.LogError(jsonEx, "Error deserializing JSON message");
                }
            };

            _channel.BasicConsume(queue: queue, autoAck: false, consumer: consumer);

            _logger.LogInformation($"Waiting for messages in queue '{queue}'...");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error receiving JSON messages from queue '{queue}'");
        }
    }

    public async Task<TResponse?> CallAsync<TRequest, TResponse>(TRequest request, string queue, TimeSpan timeout)
    {
        var correlationId = Guid.NewGuid().ToString();
        var replyQueue = _channel.QueueDeclare().QueueName;

        var props = _channel.CreateBasicProperties();
        props.ReplyTo = replyQueue;
        props.CorrelationId = correlationId;

        var message = JsonConvert.SerializeObject(request);
        var body = Encoding.UTF8.GetBytes(message);

        var tcs = new TaskCompletionSource<TResponse?>();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                try
                {
                    var responseJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var response = JsonConvert.DeserializeObject<TResponse>(responseJson);

                    if (response == null)
                    {
                        tcs.SetException(new InvalidOperationException("Received null response from RabbitMQ."));
                    }
                    else
                    {
                        tcs.SetResult(response);
                    }
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
        };

        _channel.BasicConsume(queue: replyQueue, autoAck: true, consumer: consumer);
        _channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: props, body: body);

        if (await Task.WhenAny(tcs.Task, Task.Delay(timeout)) != tcs.Task)
        {
            throw new TimeoutException("Timeout while waiting for RabbitMQ response.");
        }

        return await tcs.Task;
    }
}
