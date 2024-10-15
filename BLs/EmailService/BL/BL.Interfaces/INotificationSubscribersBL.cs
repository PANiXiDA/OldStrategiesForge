using Common.SearchParams.EmailService;
using BaseBL;
using EmailService.BL.BL.Models;

namespace EmailService.BL.BL.Interfaces;

internal interface INotificationSubscribersBL : ICrudBL<NotificationSubscriberEntity, NotificationSubscribersSearchParams, object>
{
}
