﻿using BaseDAL;
using GamesService.DAL.DbModels;
using GamesService.DAL.Interfaces;
using GameDb = GamesService.DAL.DbModels.Models.Game;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Games.SearchParams.Gen;
using Games.Entities.Gen;
using Games.ConvertParams.Gen;
using Google.Protobuf.WellKnownTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GamesService.DAL.Implementations;

public class GamesDAL : BaseDAL<DefaultDbContext, GameDb,
    Game, Guid, GamesSearchParams, GamesConvertParams>, IGamesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public GamesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Game entity,
        GameDb dbObject, bool exists)
    {
        dbObject.Id = Guid.Parse(entity.Id);
        dbObject.CreatedAt = entity.CreatedAt.ToDateTime();
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt.ToDateTime();
        dbObject.GameType = entity.GameType;
        dbObject.WinnerId = entity.WinnerId;

        return Task.CompletedTask;
    }

    protected override IQueryable<GameDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<GameDb> dbObjects, GamesSearchParams searchParams)
    {
        if (searchParams.HasGameType)
        {
            dbObjects = dbObjects.Where(item => item.GameType == searchParams.GameType);
        }
        if (searchParams.HasWinnerId)
        {
            dbObjects = dbObjects.Where(item => item.WinnerId == searchParams.WinnerId);
        }

        return dbObjects;
    }

    protected override async Task<IList<Game>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<GameDb> dbObjects, GamesConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeSessions && convertParams.IncludeSessions)
            {
                dbObjects = dbObjects.Include(item => item.Sessions);
            }
        }

        return (await dbObjects.ToListAsync()).Select(item => ConvertDbObjectToEntity(item)).ToList();
    }

    protected override Expression<Func<GameDb, Guid>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Game, Guid>> GetIdByEntityExpression()
    {
        return item => Guid.Parse(item.Id);
    }

    internal static Game ConvertDbObjectToEntity(GameDb dbObject, bool includeAdditional = true)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var game = new Game()
        {
            Id = dbObject.Id.ToString(),
            CreatedAt = dbObject.CreatedAt.ToTimestamp(),
            UpdatedAt = dbObject.UpdatedAt.ToTimestamp(),
            DeletedAt = dbObject.DeletedAt?.ToTimestamp(),
            GameType = dbObject.GameType,
            WinnerId = dbObject.WinnerId
        };

        if (includeAdditional)
        {
            game.Sessions.AddRange(dbObject.Sessions.Select(SessionsDAL.ConvertDbObjectToEntity));
        }

        return game;
    }

    public async Task CloseAsync(Guid id)
    {
        var data = GetContext();
        try
        {
            await data.Games
                .Where(game => game.Id == id)
                .SelectMany(game => game.Sessions)
                .ExecuteUpdateAsync(session => session.SetProperty(session => session.IsActive, false));
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }

    public async Task EndAsync(Guid gameId, int winnerId)
    {
        using var data = GetContext();
        using var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            await data.Games
                .Where(game => game.Id == gameId)
                .ExecuteUpdateAsync(game => game.SetProperty(game => game.WinnerId, winnerId));

            await data.Sessions
                .Where(session => session.GameId == gameId)
                .ExecuteUpdateAsync(session => session.SetProperty(session => session.IsActive, false));

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
        finally
        {
            await DisposeContextAsync(data);
        }
    }
}