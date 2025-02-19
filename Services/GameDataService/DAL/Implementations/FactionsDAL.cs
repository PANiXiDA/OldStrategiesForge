using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using FactionDb = GameDataService.DAL.DbModels.Models.Faction;

namespace GameDataService.DAL.Implementations;

public class FactionsDAL : BaseDAL<DefaultDbContext, FactionDb,
    Faction, int, FactionsSearchParams, FactionsConvertParams>, IFactionsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public FactionsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Faction entity,
        FactionDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;       

        return Task.CompletedTask;
    }

    protected override IQueryable<FactionDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<FactionDb> dbObjects, FactionsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Faction>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<FactionDb> dbObjects, FactionsConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeAbilities && convertParams.IncludeAbilities)
            {
                dbObjects = dbObjects.Include(item => item.Abilities);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<FactionDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Faction, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Faction ConvertDbObjectToEntity(FactionDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var faction = new Faction()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description
        };

        faction.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return faction;
    }
}
