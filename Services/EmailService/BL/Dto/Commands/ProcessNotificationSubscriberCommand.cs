using MediatR;

namespace EmailService.BL.Dto.Commands;

internal class NotificationSubscriberCommand : IRequest<Unit>
{
    internal string Message { get; }

    public NotificationSubscriberCommand(string message)
    {
        Message = message;
    }
}
