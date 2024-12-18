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

public class PrivateChatsDAL : BaseDAL<DefaultDbContext, PrivateChat,
    PrivateChatDto, Guid, PrivateChatsSearchParams, PrivateChatsConvertParams>, IPrivateChatsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public PrivateChatsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, PrivateChatDto entity,
        PrivateChat dbObject, bool exists)
    {
        dbObject.Id = entity.Id;
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.Player1Id = entity.Player1Id;
        dbObject.Player2Id = entity.Player2Id;

        return Task.CompletedTask;
    }

    protected override IQueryable<PrivateChat> BuildDbQuery(DefaultDbContext context,
        IQueryable<PrivateChat> dbObjects, PrivateChatsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<PrivateChatDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<PrivateChat> dbObjects, PrivateChatsConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<PrivateChat, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<PrivateChatDto, Guid>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static PrivateChatDto ConvertDbObjectToEntity(PrivateChat dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new PrivateChatDto(
            dbObject.Id,
            dbObject.CreatedAt,
            dbObject.UpdatedAt,
            dbObject.DeletedAt,
            dbObject.Player1Id,
            dbObject.Player2Id
            )
        {
        };
    }
}
