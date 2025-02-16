using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HeroDb = GameDataService.DAL.DbModels.Models.Hero;

namespace GameDataService.DAL.Implementations;

public class HeroesDAL : BaseDAL<DefaultDbContext, HeroDb,
    Hero, int, HeroesSearchParams, HeroesConvertParams>, IHeroesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public HeroesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Hero entity,
        HeroDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.Attack = entity.Attack;
        dbObject.Defence = entity.Defence;
        dbObject.MinDamage = entity.MinDamage;
        dbObject.MaxDamage = entity.MaxDamage;
        dbObject.Initiative = entity.Initiative;
        dbObject.Morale = entity.Morale;
        dbObject.Luck = entity.Luck;
        dbObject.SubfactionId = entity.SubfactionId;
        dbObject.HeroClassId = entity.HeroClassId;

        return Task.CompletedTask;
    }

    protected override IQueryable<HeroDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<HeroDb> dbObjects, HeroesSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Hero>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<HeroDb> dbObjects, HeroesConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<HeroDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Hero, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Hero ConvertDbObjectToEntity(HeroDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var hero = new Hero()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
            Attack = dbObject.Attack,
            Defence = dbObject.Defence,
            MinDamage = dbObject.MinDamage,
            MaxDamage = dbObject.MaxDamage,
            Initiative = dbObject.Initiative,
            Morale = dbObject.Morale,
            Luck = dbObject.Luck,
            SubfactionId = dbObject.SubfactionId,
            HeroClassId = dbObject.HeroClassId,
            Subfaction = dbObject.Subfaction != null ? SubfactionsDAL.ConvertDbObjectToEntity(dbObject.Subfaction) : null,
            HeroClass = dbObject.HeroClass != null ? HeroClassesDAL.ConvertDbObjectToEntity(dbObject.HeroClass) : null
        };

        hero.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));
        hero.Artefacts.AddRange(dbObject.Artefacts.Select(ArtefactsDAL.ConvertDbObjectToEntity));

        return hero;
    }
}
