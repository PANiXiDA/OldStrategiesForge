using EmailService.BL.Dto.Commands;
using MediatR;
using RabbitMQ.Client;
using Tools.RabbitMQ;
using Constants = Common.Constants;

namespace EmailService.BackgroundServices;

internal class EmailProcessingService : BackgroundService
{
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
            _rabbitMQClient.StartReceivingMultiple(new Dictionary<string, (Type, Func<object, IBasicProperties?, IModel?, Task>)>
            {
                [Constants.RabbitMqQueues.SubscribeToNotifications] = (typeof(string), async (message, _, _) =>
                {
                    var typedMessage = (string)message;

                    try
                    {
                        _logger.LogInformation($"Received email message: {typedMessage}");

                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                            await mediator.Send(new NotificationSubscriberCommand(typedMessage));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred while processing the email message.");
                    }
                }
                )
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while starting to listen for email messages from RabbitMQ.");
        }

        return Task.CompletedTask;
    }
}
