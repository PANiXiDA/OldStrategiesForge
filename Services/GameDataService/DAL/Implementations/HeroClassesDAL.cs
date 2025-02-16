using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using HeroClassDb = GameDataService.DAL.DbModels.Models.HeroClass;

namespace GameDataService.DAL.Implementations;

public class HeroClassesDAL : BaseDAL<DefaultDbContext, HeroClassDb,
    HeroClass, int, HeroClassesSearchParams, HeroClassesConvertParams>, IHeroClassesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public HeroClassesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, HeroClass entity,
        HeroClassDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;

        return Task.CompletedTask;
    }

    protected override IQueryable<HeroClassDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<HeroClassDb> dbObjects, HeroClassesSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<HeroClass>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<HeroClassDb> dbObjects, HeroClassesConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<HeroClassDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<HeroClass, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static HeroClass ConvertDbObjectToEntity(HeroClassDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var heroClass = new HeroClass()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description
        };

        heroClass.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return heroClass;
    }
}
