using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AbilityDb = GameDataService.DAL.DbModels.Models.Ability;

namespace GameDataService.DAL.Implementations;

public class AbilitiesDAL : BaseDAL<DefaultDbContext, AbilityDb,
    Ability, int, AbilitiesSearchParams, AbilitiesConvertParams>, IAbilitiesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public AbilitiesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Ability entity,
        AbilityDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;

        return Task.CompletedTask;
    }

    protected override IQueryable<AbilityDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<AbilityDb> dbObjects, AbilitiesSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Ability>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<AbilityDb> dbObjects, AbilitiesConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeEffects && convertParams.IncludeEffects)
            {
                dbObjects = dbObjects.Include(item => item.Effects);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<AbilityDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Ability, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Ability ConvertDbObjectToEntity(AbilityDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var ability = new Ability()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description
        };

        ability.Effects.AddRange(dbObject.Effects.Select(EffectsDAL.ConvertDbObjectToEntity));

        return ability;
    }
}
