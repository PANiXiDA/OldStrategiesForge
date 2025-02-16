using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SpellDb = GameDataService.DAL.DbModels.Models.Spell;


namespace GameDataService.DAL.Implementations;

public class SpellsDAL : BaseDAL<DefaultDbContext, SpellDb,
    Spell, int, SpellsSearchParams, SpellsConvertParams>, ISpellsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public SpellsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Spell entity,
        SpellDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.RequiredSkillId = entity.RequiredSkillId;

        return Task.CompletedTask;
    }

    protected override IQueryable<SpellDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<SpellDb> dbObjects, SpellsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Spell>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<SpellDb> dbObjects, SpellsConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<SpellDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Spell, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Spell ConvertDbObjectToEntity(SpellDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var spell = new Spell()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
            RequiredSkillId = dbObject.RequiredSkillId,

            RequiredSkill = dbObject.RequiredSkill != null ? SkillsDAL.ConvertDbObjectToEntity(dbObject.RequiredSkill) : null
        };

        spell.Abilities.AddRange(dbObject.Abilities.Select(AbilitiesDAL.ConvertDbObjectToEntity));

        return spell;
    }
}
