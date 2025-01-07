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

public class GamesDAL : BaseDAL<DefaultDbContext, Game,
    GameDto, Guid, GamesSearchParams, GamesConvertParams>, IGamesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public GamesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, GameDto entity,
        Game dbObject, bool exists)
    {
        dbObject.Id = entity.Id;
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.GameType = entity.GameType;
        dbObject.WinnerId = entity.WinnerId;

        return Task.CompletedTask;
    }

    protected override IQueryable<Game> BuildDbQuery(DefaultDbContext context,
        IQueryable<Game> dbObjects, GamesSearchParams searchParams)
    {
        dbObjects = dbObjects.OrderBy(item => item.CreatedAt);

        return dbObjects;
    }

    protected override async Task<IList<GameDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Game> dbObjects, GamesConvertParams? convertParams, bool isFull)
    {
        dbObjects = dbObjects.Include(item => item.Sessions);

        return (await dbObjects.ToListAsync()).Select(item => ConvertDbObjectToEntity(item)).ToList();
    }

    protected override Expression<Func<Game, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<GameDto, Guid>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static GameDto ConvertDbObjectToEntity(Game dbObject, bool isIncludeAdditional = true)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new GameDto(
            dbObject.Id,
            dbObject.CreatedAt,
            dbObject.UpdatedAt,
            dbObject.DeletedAt,
            dbObject.GameType,
            dbObject.WinnerId
            )
        {
            Sessions = isIncludeAdditional ? dbObject.Sessions.Select(SessionsDAL.ConvertDbObjectToEntity).ToList() : null
        };
    }
}