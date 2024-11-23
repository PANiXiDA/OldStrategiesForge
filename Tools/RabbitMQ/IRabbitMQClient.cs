using RabbitMQ.Client;

namespace Tools.RabbitMQ;

public interface IRabbitMQClient
{
    void SendMessage<T>(T messageObject, string queue);
    void StartReceivingMultiple(Dictionary<string, (Type messageType, Func<object, IBasicProperties?, IModel?, Task> handler)> queueHandlers);
    Task<TResponse?> CallAsync<TRequest, TResponse>(TRequest request, string queue, TimeSpan timeout);
}
