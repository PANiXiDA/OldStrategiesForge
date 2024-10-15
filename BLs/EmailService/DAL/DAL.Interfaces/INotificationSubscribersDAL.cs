using BaseDAL;
using Common.SearchParams.EmailService;
using EmailService.BL.BL.Models;
using EmailService.DAL.DAL.DbModels;
using EmailService.DAL.DAL.DbModels.Models;

namespace EmailService.DAL.DAL.Interfaces;

internal interface INotificationSubscribersDAL : IBaseDal<DefaultDbContext, NotificationSubscriberDbModel,
    NotificationSubscriberEntity, int, NotificationSubscribersSearchParams, object>
{
}


