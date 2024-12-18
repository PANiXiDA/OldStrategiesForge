using EmailService.BL.Dto.Commands;
using MediatR;
using System.Text;
using RabbitMQ.Client;
using Tools.RabbitMQ;
using static EmailService.BL.Dto.Commands.ProcessEmailSenderCommand;
using System.Text.Json;
using Common.Dto.RabbitMq;
using Constants = Common.Constants;

namespace EmailService.BackgroundServices;

internal class SendEmailProcessingService : BackgroundService
{
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
                [Constants.RabbitMqQueues.ConfirmEmail] = (typeof(SendEmailRequest), async (message, props, channel) =>
                {
                    var typedMessage = (SendEmailRequest)message;
                    await ProcessMessageAsync(typedMessage, props, channel!, async (msg, provider) =>
                    {
                        var mediator = provider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(
                            email: msg.Email,
                            type: EmailType.Confirmation,
                            playerId: msg.Id));

                        return (result.IsSuccess, null);
                    });
                }
                ),
                [Constants.RabbitMqQueues.RecoveryPassword] = (typeof(SendEmailRequest), async (message, props, channel) =>
                {
                    var typedMessage = (SendEmailRequest)message;
                    await ProcessMessageAsync(typedMessage, props, channel!, async (msg, provider) =>
                    {
                        var mediator = provider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(
                            email: msg.Email,
                            type: EmailType.RecoveryPassword,
                            playerId: msg.Id));

                        return (result.IsSuccess, null);
                    });
                }
                ),
                [Common.Constants.RabbitMqQueues.ChangePassword] = (typeof(SendEmailRequest), async(message, props, channel) =>
                {
                    var typedMessage = (SendEmailRequest)message;
                    await ProcessMessageAsync(typedMessage, props, channel!, async (msg, provider) =>
                    {
                        var mediator = provider.GetRequiredService<IMediator>();
                        var result = await mediator.Send(new ProcessEmailSenderCommand(
                            email: msg.Email,
                            type: EmailType.ChangedPassword,
                            playerId: null,
                            newPassword: msg.Password));

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
                    SendReply(channel, props, result.IsSuccess, result.ErrorMessage);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing the message.");

            if (!string.IsNullOrWhiteSpace(props?.ReplyTo))
            {
                SendReply(channel, props, false, ex.Message);
            }
        }
    }

    private void SendReply(IModel channel, IBasicProperties props, bool isSuccess, string? errorMessage)
    {
        var responseProps = channel.CreateBasicProperties();
        responseProps.CorrelationId = props.CorrelationId;

        var responseMessage = JsonSerializer.Serialize(new { Success = isSuccess, Error = errorMessage });
        var responseBody = Encoding.UTF8.GetBytes(responseMessage);

        channel.BasicPublish(
            exchange: "",
            routingKey: props.ReplyTo,
            basicProperties: responseProps,
            body: responseBody);

        _logger.LogInformation($"Response sent back to ReplyTo queue: {responseMessage}");
    }
}
