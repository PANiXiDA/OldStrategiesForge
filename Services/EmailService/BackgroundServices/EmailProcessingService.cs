using EmailService.BL.Dto.Commands;
using MediatR;
using Tools.RabbitMQ;

namespace EmailService.BackgroundServices;

internal class EmailProcessingService : BackgroundService
{
    private const string _queue = "subscribe_to_notifications_requests";
    private readonly ILogger<EmailProcessingService> _logger;
    private readonly IRabbitMQClient _rabbitMQClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;

    public EmailProcessingService(
        ILogger<EmailProcessingService> logger,
        IRabbitMQClient rabbitMQClient,
        IServiceProvider serviceProvider,
        IMediator mediator)
    {
        _rabbitMQClient = rabbitMQClient;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _mediator = mediator;
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
                    _logger.LogInformation($"Received email message: {message}");
                     
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Send(new NotificationSubscriberCommand(message));
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
