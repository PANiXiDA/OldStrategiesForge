using BaseDAL;
using Common.ConvertParams.GamesService;
using Common.SearchParams.GamesService;
using GamesService.DAL.DbModels;
using GamesService.DAL.DbModels.Models;
using GamesService.DAL.Interfaces;
using GamesService.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GamesService.DAL.Implementations;

public class SessionsDAL : BaseDAL<DefaultDbContext, Session,
    SessionDto, Guid, SessionsSearchParams, SessionsConvertParams>, ISessionsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public SessionsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, SessionDto entity,
        Session dbObject, bool exists)
    {
        dbObject.Id = entity.Id;
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.PlayerId = entity.PlayerId;
        dbObject.GameId = entity.GameId;
        dbObject.IsActive = entity.IsActive;

        return Task.CompletedTask;
    }

    protected override IQueryable<Session> BuildDbQuery(DefaultDbContext context,
        IQueryable<Session> dbObjects, SessionsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<SessionDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Session> dbObjects, SessionsConvertParams? convertParams, bool isFull)
    {
        dbObjects = dbObjects.Include(item => item.Game);

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Session, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<SessionDto, Guid>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static SessionDto ConvertDbObjectToEntity(Session dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new SessionDto(
            dbObject.Id,
            dbObject.CreatedAt,
            dbObject.UpdatedAt,
            dbObject.DeletedAt,
            dbObject.PlayerId,
            dbObject.GameId,
            dbObject.IsActive
            )
        {
            Game = dbObject.Game != null ? GamesDAL.ConvertDbObjectToEntity(dbObject.Game, false) : null
        };
    }
}
