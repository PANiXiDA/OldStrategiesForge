using EmailService.BL.Dto.Commands;
using MediatR;
using Newtonsoft.Json;
using System.Text;
using RabbitMQ.Client;
using Tools.RabbitMQ;
using static EmailService.BL.Dto.Commands.ProcessEmailSenderCommand;

namespace EmailService.BackgroundServices;

internal class SendEmailProcessingService : BackgroundService
{
    private const string _email_confirm_queue = "email_confirm";
    private const string _recovery_password_queue = "recovery_password";
    private const string _change_password_queue = "change_password";

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
            _rabbitMQClient.StartReceivingMultiple(new Dictionary<string, (Type, Func<object, IBasicProperties?, IModel?, Task>)>
            {
                [_email_confirm_queue] = (typeof((string, int)), async (message, props, channel) =>
                {
                    var typedMessage = ((string, int))message;
                    await ProcessMessageAsync(typedMessage, props, channel!, async (msg, provider) =>
                    {
                        var mediator = provider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(
                            email: msg.Item1,
                            type: EmailType.Confirmation,
                            playerId: msg.Item2));

                        return (result.IsSuccess, null);
                    });
                }
                ),
                [_recovery_password_queue] = (typeof((string, int)), async (message, props, channel) =>
                {
                    var typedMessage = ((string, int))message;
                    await ProcessMessageAsync(typedMessage, props, channel!, async (msg, provider) =>
                    {
                        var mediator = provider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(
                            email: msg.Item1,
                            type: EmailType.RecoveryPassword,
                            playerId: msg.Item2));

                        return (result.IsSuccess, null);
                    });
                }
                ),
                [_change_password_queue] = (typeof((string, string)), async(message, props, channel) =>
                {
                    var typedMessage = ((string, string))message;
                    await ProcessMessageAsync(typedMessage, props, channel!, async (msg, provider) =>
                    {
                        var mediator = provider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(
                            email: msg.Item1,
                            type: EmailType.ChangedPassword,
                            playerId: null,
                            newPassword: msg.Item2));

                        return (result.IsSuccess, null);
                    });
                }
                )
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while starting to listen for email sender messages from RabbitMQ.");
        }

        return Task.CompletedTask;
    }


    private async Task ProcessMessageAsync<T>(
        T message,
        IBasicProperties? props,
        IModel channel,
        Func<T, IServiceProvider, Task<(bool IsSuccess, string? ErrorMessage)>> processLogic)
    {
        try
        {
            _logger.LogInformation($"Processing message: {message}");

            using (var scope = _serviceProvider.CreateScope())
            {
                var result = await processLogic(message, scope.ServiceProvider);

                if (!string.IsNullOrWhiteSpace(props?.ReplyTo))
                {
                    var responseProps = channel.CreateBasicProperties();
                    responseProps.CorrelationId = props.CorrelationId;

                    var responseMessage = JsonConvert.SerializeObject(result.IsSuccess);
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
            _logger.LogError(ex, "Error occurred while processing the message.");
        }
    }
}
