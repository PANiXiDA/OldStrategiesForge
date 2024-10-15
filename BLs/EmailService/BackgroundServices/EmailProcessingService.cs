using EmailService.BL.BL.Interfaces;
using EmailService.BL.BL.Models;
using EmailService.DAL.DAL.DbModels;
using Tools.RabbitMQ;

namespace EmailService.BackgroundServices;

public class EmailProcessingService : BackgroundService
{
    private const string _queue = "subscribe_to_notifications_requests";
    private readonly ILogger<EmailProcessingService> _logger;
    private readonly IRabbitMQClient _rabbitMQClient;
    private readonly IServiceProvider _serviceProvider;

    public EmailProcessingService(
        ILogger<EmailProcessingService> logger,
        IRabbitMQClient rabbitMQClient,
        IServiceProvider serviceProvider)
    {
        _rabbitMQClient = rabbitMQClient;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting to listen for email messages from RabbitMQ...");

        try
        {
            _rabbitMQClient.StartReceiving<string>(async message =>
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();

                        _logger.LogInformation($"Received email message: {message}");
                        var notificationSubscribersBL = scope.ServiceProvider.GetRequiredService<INotificationSubscribersBL>();

                        var notificationSubscriber = new NotificationSubscriberEntity(message);
                        await notificationSubscribersBL.AddOrUpdateAsync(notificationSubscriber);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing the email message.");
                }
            }, _queue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while starting to listen for email messages from RabbitMQ.");
        }

        return Task.CompletedTask;
    }
}
