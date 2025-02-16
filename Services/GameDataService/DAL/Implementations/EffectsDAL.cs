using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EffectDb = GameDataService.DAL.DbModels.Models.Effect;

namespace GameDataService.DAL.Implementations;

public class EffectsDAL : BaseDAL<DefaultDbContext, EffectDb,
    Effect, int, EffectsSearchParams, EffectsConvertParams>, IEffectsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public EffectsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Effect entity,
        EffectDb dbObject, bool exists)
    {
        dbObject.EffectType = entity.EffectType;
        dbObject.Value = entity.Value;
        dbObject.Duration = entity.Duration;
        dbObject.Parameters = entity.Parameters;

        return Task.CompletedTask;
    }

    protected override IQueryable<EffectDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<EffectDb> dbObjects, EffectsSearchParams searchParams)
    {
        if (searchParams.HasEffectType)
        {
            dbObjects = dbObjects.Where(item => item.EffectType == searchParams.EffectType);
        }

        return dbObjects;
    }

    protected override async Task<IList<Effect>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<EffectDb> dbObjects, EffectsConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<EffectDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Effect, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Effect ConvertDbObjectToEntity(EffectDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var effect = new Effect()
        {
            Id = dbObject.Id,
            EffectType = dbObject.EffectType,
            Value = dbObject.Value,
            Duration = dbObject.Duration,
            Parameters = dbObject.Parameters,
        };

        return effect;
    }
}
