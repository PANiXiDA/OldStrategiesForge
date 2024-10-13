namespace Tools.RabbitMQ
{
    public interface IRabbitMQClient
    {
        void SendMessage<T>(T messageObject, string queue);
        void StartReceiving<T>(Action<T> handleMessage, string queue);
    }
}
