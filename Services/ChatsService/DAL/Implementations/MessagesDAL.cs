using BaseDAL;
using ChatsService.DAL.DbModels;
using ChatsService.DAL.DbModels.Models;
using ChatsService.DAL.Interfaces;
using ChatsService.Dto;
using Common.ConvertParams.ChatsService;
using Common.SearchParams.ChatsService;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatsService.DAL.Implementations;

public class MessagesDAL : BaseDAL<DefaultDbContext, Message,
    MessageDto, Guid, MessagesSearchParams, MessagesConvertParams>, IMessagesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public MessagesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, MessageDto entity,
        Message dbObject, bool exists)
    {
        dbObject.Id = entity.Id;
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.ChatId = entity.ChatId;
        dbObject.SenderId = entity.SenderId;
        dbObject.Content = entity.Content;
        dbObject.IsRead = entity.IsRead;

        return Task.CompletedTask;
    }

    protected override IQueryable<Message> BuildDbQuery(DefaultDbContext context,
        IQueryable<Message> dbObjects, MessagesSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<MessageDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Message> dbObjects, MessagesConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Message, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<MessageDto, Guid>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static MessageDto ConvertDbObjectToEntity(Message dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new MessageDto(
            dbObject.Id,
            dbObject.CreatedAt,
            dbObject.UpdatedAt,
            dbObject.DeletedAt,
            dbObject.ChatId,
            dbObject.SenderId,
            dbObject.Content,
            dbObject.IsRead
            )
        {
        };
    }
}
