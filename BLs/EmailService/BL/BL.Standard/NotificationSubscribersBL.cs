using Common.SearchParams.Core;
using Common.SearchParams.EmailService;
using EmailService.BL.BL.Interfaces;
using EmailService.BL.BL.Models;
using EmailService.DAL.DAL.Interfaces;

namespace EmailService.BL.BL.Standard;

public class NotificationSubscribersBL : INotificationSubscribersBL
{
    private readonly INotificationSubscribersDAL _notificationSubscribersDAL;

    public NotificationSubscribersBL(INotificationSubscribersDAL notificationSubscribersDAL)
    {
        _notificationSubscribersDAL = notificationSubscribersDAL;
    }

    public async Task<int> AddOrUpdateAsync(NotificationSubscriberEntity entity)
    {
        entity.Id = await _notificationSubscribersDAL.AddOrUpdateAsync(entity);

        return entity.Id;
    }

    public Task<bool> ExistsAsync(int id)
    {
        return _notificationSubscribersDAL.ExistsAsync(id);
    }

    public Task<bool> ExistsAsync(NotificationSubscribersSearchParams searchParams)
    {
        return _notificationSubscribersDAL.ExistsAsync(searchParams);
    }

    public Task<NotificationSubscriberEntity> GetAsync(int id, object? convertParams = null)
    {
        return _notificationSubscribersDAL.GetAsync(id, convertParams);
    }

    public Task<bool> DeleteAsync(int id)
    {
        return _notificationSubscribersDAL.DeleteAsync(id);
    }

    public Task<SearchResult<NotificationSubscriberEntity>> GetAsync(NotificationSubscribersSearchParams searchParams, object? convertParams = null)
    {
        return _notificationSubscribersDAL.GetAsync(searchParams, convertParams);
    }
}
