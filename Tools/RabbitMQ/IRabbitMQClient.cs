using RabbitMQ.Client;

namespace Tools.RabbitMQ;

public interface IRabbitMQClient
{
    void SendMessage<T>(T messageObject, string queue);
    void StartReceiving<T>(Func<T, Task> handleMessage, string queue);
    void StartReceiving<T>(Func<T, IBasicProperties, IModel, Task> handleMessage, string queue);
    Task<TResponse?> CallAsync<TRequest, TResponse>(TRequest request, string queue, TimeSpan timeout);
}
