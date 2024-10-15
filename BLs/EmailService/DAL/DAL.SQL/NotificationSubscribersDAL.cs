using BaseDAL;
using Common.SearchParams.EmailService;
using EmailService.BL.BL.Models;
using EmailService.DAL.DAL.DbModels;
using EmailService.DAL.DAL.DbModels.Models;
using EmailService.DAL.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EmailService.DAL.DAL.SQL;

public class NotificationSubscribersDAL : BaseDAL<DefaultDbContext, NotificationSubscriberDbModel,
    NotificationSubscriberEntity, int, NotificationSubscribersSearchParams, object>, INotificationSubscribersDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public NotificationSubscribersDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, NotificationSubscriberEntity entity,
        NotificationSubscriberDbModel dbObject, bool exists)
    {
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = entity.UpdatedAt;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.Email = entity.Email;

        return Task.CompletedTask;
    }

    protected override IQueryable<NotificationSubscriberDbModel> BuildDbQuery(DefaultDbContext context,
        IQueryable<NotificationSubscriberDbModel> dbObjects, NotificationSubscribersSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<NotificationSubscriberEntity>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<NotificationSubscriberDbModel> dbObjects, object? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<NotificationSubscriberDbModel, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<NotificationSubscriberEntity, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static NotificationSubscriberEntity ConvertDbObjectToEntity(NotificationSubscriberDbModel dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new NotificationSubscriberEntity(
            dbObject.Id,
            dbObject.CreatedAt,
            dbObject.UpdatedAt,
            dbObject.DeletedAt,
            dbObject.Email
        );
    }
}
