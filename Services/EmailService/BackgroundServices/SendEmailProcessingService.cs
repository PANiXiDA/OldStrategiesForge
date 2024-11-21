using EmailService.BL.Dto.Commands;
using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using Tools.RabbitMQ;

namespace EmailService.BackgroundServices;

internal class SendEmailProcessingService : BackgroundService
{
    private const string _queue = "email_sender_requests";

    private readonly ILogger<SendEmailProcessingService> _logger;
    private readonly IRabbitMQClient _rabbitMQClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMediator _mediator;

    public SendEmailProcessingService(
    ILogger<SendEmailProcessingService> logger,
    IRabbitMQClient rabbitMQClient,
    IServiceProvider serviceProvider,
    IMediator mediator)
    {
        _logger = logger;
        _rabbitMQClient = rabbitMQClient;
        _serviceProvider = serviceProvider;
        _mediator = mediator;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting to listen for email sender messages from RabbitMQ...");

        try
        {
            _rabbitMQClient.StartReceiving<(string, int)>(async (message, props, channel) =>
            {
                try
                {
                    _logger.LogInformation($"Received email sender message: {message}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(message.Item1, message.Item2));

                        if (!string.IsNullOrWhiteSpace(props.ReplyTo))
                        {
                            var responseProps = channel.CreateBasicProperties();
                            responseProps.CorrelationId = props.CorrelationId;

                            var responseMessage = JsonConvert.SerializeObject("Email sent successfully"); ;
                            var responseBody = Encoding.UTF8.GetBytes(responseMessage);

                            channel.BasicPublish(
                                exchange: "",
                                routingKey: props.ReplyTo,
                                basicProperties: responseProps,
                                body: responseBody);

                            _logger.LogInformation("Response sent back to ReplyTo queue.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing the email sender message.");
                }
            }, _queue);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while starting to listen for email sender messages from RabbitMQ.");
        }

        return Task.CompletedTask;
    }
}
