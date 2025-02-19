using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ArtefactSetBonusDb = GameDataService.DAL.DbModels.Models.ArtefactSetBonus;

namespace GameDataService.DAL.Implementations;

public class ArtefactSetBonusesDAL : BaseDAL<DefaultDbContext, ArtefactSetBonusDb,
    ArtefactSetBonus, int, ArtefactSetBonusesSearchParams, ArtefactSetBonusesConvertParams>, IArtefactSetBonusesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public ArtefactSetBonusesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, ArtefactSetBonus entity,
        ArtefactSetBonusDb dbObject, bool exists)
    {
        dbObject.AttackBonus = entity.AttackBonus;
        dbObject.DefenceBonus = entity.DefenceBonus;
        dbObject.HealthBonus = entity.HealthBonus;
        dbObject.MinDamageBonus = entity.MinDamageBonus;
        dbObject.MaxDamageBonus = entity.MaxDamageBonus;
        dbObject.InitiativeBonus = entity.InitiativeBonus;
        dbObject.SpeedBonus = entity.SpeedBonus;
        dbObject.RangeBonus = entity.RangeBonus;
        dbObject.ArrowsBonus = entity.ArrowsBonus;
        dbObject.MoraleBonus = entity.MoraleBonus;
        dbObject.LuckBonus = entity.LuckBonus;
        dbObject.HeroClassId = entity.HeroClassId;
        dbObject.ArtefactSetId = entity.ArtefactSetId;

        return Task.CompletedTask;
    }

    protected override IQueryable<ArtefactSetBonusDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<ArtefactSetBonusDb> dbObjects, ArtefactSetBonusesSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<ArtefactSetBonus>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<ArtefactSetBonusDb> dbObjects, ArtefactSetBonusesConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeArtefactSet && convertParams.IncludeArtefactSet)
            {
                dbObjects = dbObjects.Include(item => item.ArtefactSet);
            }
            if (convertParams.HasIncludeHeroClass && convertParams.IncludeHeroClass)
            {
                dbObjects = dbObjects.Include(item => item.HeroClass);
            }
            if (convertParams.HasIncludeAbilities && convertParams.IncludeAbilities)
            {
                dbObjects = dbObjects.Include(item => item.Abilities);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<ArtefactSetBonusDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<ArtefactSetBonus, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static ArtefactSetBonus ConvertDbObjectToEntity(ArtefactSetBonusDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var atefactSetBonus = new ArtefactSetBonus()
        {
            Id = dbObject.Id,
            AttackBonus = dbObject.AttackBonus,
            DefenceBonus = dbObject.DefenceBonus,
            HealthBonus = dbObject.HealthBonus,
            MinDamageBonus = dbObject.MinDamageBonus,
            MaxDamageBonus = dbObject.MaxDamageBonus,
            InitiativeBonus = dbObject.InitiativeBonus,
            SpeedBonus = dbObject.SpeedBonus,
            RangeBonus = dbObject.RangeBonus,
            ArrowsBonus = dbObject.ArrowsBonus,
            MoraleBonus = dbObject.MoraleBonus,
            LuckBonus = dbObject.LuckBonus,
            HeroClassId = dbObject.HeroClassId,
            ArtefactSetId = dbObject.ArtefactSetId,

            HeroClass = dbObject.HeroClass != null ? HeroClassesDAL.ConvertDbObjectToEntity(dbObject.HeroClass) : null,
            ArtefactSet = dbObject.ArtefactSet != null ? ArtefactSetsDAL.ConvertDbObjectToEntity(dbObject.ArtefactSet) : null
        };

        atefactSetBonus.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return atefactSetBonus;
    }
}
