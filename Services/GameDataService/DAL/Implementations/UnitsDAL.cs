using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UnitDb = GameDataService.DAL.DbModels.Models.Unit;

namespace GameDataService.DAL.Implementations;

public class UnitsDAL : BaseDAL<DefaultDbContext, UnitDb,
    Unit, int, UnitsSearchParams, UnitsConvertParams>, IUnitsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public UnitsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Unit entity,
        UnitDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.Attack = entity.Attack;
        dbObject.Defence = entity.Defence;
        dbObject.Health = entity.Health;
        dbObject.MinDamage = entity.MinDamage;
        dbObject.MaxDamage = entity.MaxDamage;
        dbObject.Initiative = entity.Initiative;
        dbObject.Speed = entity.Speed;
        dbObject.Range = entity.Range;
        dbObject.Arrows = entity.Arrows;
        dbObject.Morale = entity.Morale;
        dbObject.Luck = entity.Luck;
        dbObject.FactionId = entity.FactionId;
        dbObject.BaseUnitId = entity.BaseUnitId;

        return Task.CompletedTask;
    }

    protected override IQueryable<UnitDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<UnitDb> dbObjects, UnitsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Unit>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<UnitDb> dbObjects, UnitsConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeBaseUnit && convertParams.IncludeBaseUnit)
            {
                dbObjects = dbObjects.Include(item => item.BaseUnit);
            }
            if (convertParams.HasIncludeFaction && convertParams.IncludeFaction)
            {
                dbObjects = dbObjects.Include(item => item.Faction);
            }
            if (convertParams.HasIncludeAbilities && convertParams.IncludeAbilities)
            {
                dbObjects = dbObjects.Include(item => item.Abilities);
            }
            if (convertParams.HasIncludeUpgrades && convertParams.IncludeUpgrades)
            {
                dbObjects = dbObjects.Include(item => item.Upgrades);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<UnitDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Unit, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Unit ConvertDbObjectToEntity(UnitDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var unit = new Unit()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
            Attack = dbObject.Attack,
            Defence = dbObject.Defence,
            Health = dbObject.Health,
            MinDamage = dbObject.MinDamage,
            MaxDamage = dbObject.MaxDamage,
            Initiative = dbObject.Initiative,
            Speed = dbObject.Speed,
            Range = dbObject.Range,
            Arrows = dbObject.Arrows,
            Morale = dbObject.Morale,
            Luck = dbObject.Luck,
            FactionId = dbObject.FactionId,
            BaseUnitId = dbObject.BaseUnitId,

            Faction = dbObject.Faction != null ? FactionsDAL.ConvertDbObjectToEntity(dbObject.Faction) : null,
            BaseUnit = dbObject.BaseUnit != null ? ConvertDbObjectToEntity(dbObject.BaseUnit) : null,
        };

        unit.Upgrades.AddRange(dbObject.Upgrades.Select(ConvertDbObjectToEntity));
        unit.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return unit;
    }
}
