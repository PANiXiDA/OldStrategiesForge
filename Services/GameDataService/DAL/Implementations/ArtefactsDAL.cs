using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ArtefactDb = GameDataService.DAL.DbModels.Models.Artefact;

namespace GameDataService.DAL.Implementations;

public class ArtefactsDAL : BaseDAL<DefaultDbContext, ArtefactDb,
    Artefact, int, ArtefactsSearchParams, ArtefactsConvertParams>, IArtefactsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public ArtefactsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Artefact entity,
        ArtefactDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
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
        dbObject.ArtefactSlot = entity.ArtefactSlot;
        dbObject.HeroClassId = entity.HeroClassId;
        dbObject.ArtefactSetId = entity.ArtefactSetId;

        return Task.CompletedTask;
    }

    protected override IQueryable<ArtefactDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<ArtefactDb> dbObjects, ArtefactsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Artefact>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<ArtefactDb> dbObjects, ArtefactsConvertParams? convertParams, bool isFull)
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

    protected override Expression<Func<ArtefactDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Artefact, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Artefact ConvertDbObjectToEntity(ArtefactDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var artefact = new Artefact()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
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
            ArtefactSlot = dbObject.ArtefactSlot,
            HeroClassId = dbObject.HeroClassId,
            ArtefactSetId = dbObject.ArtefactSetId,

            HeroClass = dbObject.HeroClass != null ? HeroClassesDAL.ConvertDbObjectToEntity(dbObject.HeroClass) : null,
            ArtefactSet = dbObject.ArtefactSet != null ? ArtefactSetsDAL.ConvertDbObjectToEntity(dbObject.ArtefactSet) : null
        };

        artefact.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return artefact;
    }
}
