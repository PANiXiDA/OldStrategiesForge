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

internal class ChatsDAL : BaseDAL<DefaultDbContext, Chat,
    ChatDto, Guid, ChatsSearchParams, ChatsConvertParams>, IChatsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public ChatsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, ChatDto entity,
        Chat dbObject, bool exists)
    {
        dbObject.Id = entity.Id;
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.ChatType = entity.ChatType;
        dbObject.Name = entity.Name;

        return Task.CompletedTask;
    }

    protected override IQueryable<Chat> BuildDbQuery(DefaultDbContext context,
        IQueryable<Chat> dbObjects, ChatsSearchParams searchParams)
    {
        if (searchParams.ChatType.HasValue)
        {
            dbObjects = dbObjects.Where(item => item.ChatType == searchParams.ChatType.Value);
        }

        return dbObjects;
    }

    protected override async Task<IList<ChatDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Chat> dbObjects, ChatsConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Chat, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<ChatDto, Guid>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static ChatDto ConvertDbObjectToEntity(Chat dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new ChatDto(
            dbObject.Id,
            dbObject.CreatedAt,
            dbObject.UpdatedAt,
            dbObject.DeletedAt,
            dbObject.ChatType,
            dbObject.Name
            )
        {
        };
    }
}
