using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SubfactionDb = GameDataService.DAL.DbModels.Models.Subfaction;

namespace GameDataService.DAL.Implementations;

public class SubfactionsDAL : BaseDAL<DefaultDbContext, SubfactionDb,
    Subfaction, int, SubfactionsSearchParams, SubfactionsConvertParams>, ISubfactionsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public SubfactionsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Subfaction entity,
        SubfactionDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.FactionId = entity.FactionId;

        return Task.CompletedTask;
    }

    protected override IQueryable<SubfactionDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<SubfactionDb> dbObjects, SubfactionsSearchParams searchParams)
    {
        if (searchParams.HasFactionId)
        {
            dbObjects = dbObjects.Include(item => item.FactionId == searchParams.FactionId);
        }

        return dbObjects;
    }

    protected override async Task<IList<Subfaction>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<SubfactionDb> dbObjects, SubfactionsConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeAbilities && convertParams.IncludeAbilities)
            {
                dbObjects = dbObjects.Include(item => item.Abilities);
            }
            if (convertParams.HasIncludeFaction && convertParams.IncludeFaction)
            {
                dbObjects = dbObjects.Include(item => item.Faction);
            }
            if (convertParams.HasIncludeSkills && convertParams.IncludeSkills)
            {
                dbObjects = dbObjects.Include(item => item.Skills);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<SubfactionDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Subfaction, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Subfaction ConvertDbObjectToEntity(SubfactionDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var subfaction = new Subfaction()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
            FactionId = dbObject.FactionId,
            Faction = dbObject.Faction != null ? FactionsDAL.ConvertDbObjectToEntity(dbObject.Faction) : null
        };

        subfaction.Skills.AddRange(dbObject.Skills.Select(SkillsDAL.ConvertDbObjectToEntity));
        subfaction.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return subfaction;
    }
}
