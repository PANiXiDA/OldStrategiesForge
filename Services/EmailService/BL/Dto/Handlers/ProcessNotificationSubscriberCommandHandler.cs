using EmailService.BL.BL.Interfaces;
using EmailService.BL.BL.Models;
using EmailService.BL.Dto.Commands;
using MediatR;

namespace EmailService.BL.Dto.Handlers;

internal class NotificationSubscriberCommandHandler : IRequestHandler<NotificationSubscriberCommand, Unit>
{
    private readonly INotificationSubscribersBL _notificationSubscribersBL;

    public NotificationSubscriberCommandHandler(INotificationSubscribersBL notificationSubscribersBL)
    {
        _notificationSubscribersBL = notificationSubscribersBL;
    }

    public async Task<Unit> Handle(NotificationSubscriberCommand request, CancellationToken cancellationToken)
    {
        var notificationSubscriber = new NotificationSubscriberEntity(request.Message);
        await _notificationSubscribersBL.AddOrUpdateAsync(notificationSubscriber);

        return Unit.Value;
    }
}
