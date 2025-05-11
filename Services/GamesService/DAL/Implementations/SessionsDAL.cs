using BaseDAL;
using GamesService.DAL.DbModels;
using GamesService.DAL.Interfaces;
using SessionDb = GamesService.DAL.DbModels.Models.Session;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Games.Entities.Gen;
using Games.SearchParams.Gen;
using Games.ConvertParams.Gen;
using Google.Protobuf.WellKnownTypes;

namespace GamesService.DAL.Implementations;

public class SessionsDAL : BaseDAL<DefaultDbContext, SessionDb,
    Session, Guid, SessionsSearchParams, SessionsConvertParams>, ISessionsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public SessionsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Session entity,
        SessionDb dbObject, bool exists)
    {
        dbObject.Id = Guid.Parse(entity.Id);
        dbObject.CreatedAt = entity.CreatedAt.ToDateTime();
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt?.ToDateTime();
        dbObject.PlayerId = entity.PlayerId;
        dbObject.GameId = Guid.Parse(entity.GameId);
        dbObject.IsActive = entity.IsActive;

        return Task.CompletedTask;
    }

    protected override IQueryable<SessionDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<SessionDb> dbObjects, SessionsSearchParams searchParams)
    {
        if (searchParams.HasPlayerId)
        {
            dbObjects = dbObjects.Where(item => item.PlayerId == searchParams.PlayerId);
        }
        if (searchParams.HasGameId)
        {
            dbObjects = dbObjects.Where(item => item.GameId == Guid.Parse(searchParams.GameId));
        }
        if (searchParams.HasIsActive)
        {
            dbObjects = dbObjects.Where(item => item.IsActive == searchParams.IsActive);
        }

        return dbObjects;
    }

    protected override async Task<IList<Session>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<SessionDb> dbObjects, SessionsConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeGame && convertParams.IncludeGame)
            {
                dbObjects = dbObjects.Include(item => item.Game);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<SessionDb, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Session, Guid>> GetIdByEntityExpression()
    {
        return item => Guid.Parse(item.Id);
    }

    internal static Session ConvertDbObjectToEntity(SessionDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var session = new Session()
        {
            Id = dbObject.Id.ToString(),
            CreatedAt = dbObject.CreatedAt.ToTimestamp(),
            UpdatedAt = dbObject.UpdatedAt.ToTimestamp(),
            DeletedAt = dbObject.DeletedAt?.ToTimestamp(),
            PlayerId = dbObject.PlayerId,
            GameId = dbObject.GameId.ToString(),
            IsActive = dbObject.IsActive,

            Game = dbObject.Game != null ? GamesDAL.ConvertDbObjectToEntity(dbObject.Game, false) : null
        };

        return session;
    }
}
