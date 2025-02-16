using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using SkillDb = GameDataService.DAL.DbModels.Models.Skill;

namespace GameDataService.DAL.Implementations;

public class SkillsDAL : BaseDAL<DefaultDbContext, SkillDb,
    Skill, int, SkillsSearchParams, SkillsConvertParams>, ISkillsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public SkillsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Skill entity,
        SkillDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.SkillType = entity.SkillType;
        dbObject.CompetenceId = entity.CompetenceId;
        dbObject.SubfactionId = entity.SubfactionId;
        dbObject.AbilityId = entity.AbilityId;

        return Task.CompletedTask;
    }

    protected override IQueryable<SkillDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<SkillDb> dbObjects, SkillsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Skill>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<SkillDb> dbObjects, SkillsConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<SkillDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Skill, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Skill ConvertDbObjectToEntity(SkillDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var skill = new Skill()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
            SkillType = dbObject.SkillType,
            CompetenceId = dbObject.CompetenceId,
            SubfactionId = dbObject.SubfactionId,
            AbilityId = dbObject.AbilityId,

            Competence = dbObject.Competence != null ? CompetenciesDAL.ConvertDbObjectToEntity(dbObject.Competence) : null,
            Subfaction = dbObject.Subfaction != null ? SubfactionsDAL.ConvertDbObjectToEntity(dbObject.Subfaction) : null,
            Ability = dbObject.Ability != null ? AbilitiesDAL.ConvertDbObjectToEntity(dbObject.Ability) : null
        };

        skill.RequiredSkills.AddRange(dbObject.RequiredSkills.Select(ConvertDbObjectToEntity));
        skill.DependentSkills.AddRange(dbObject.DependentSkills.Select(ConvertDbObjectToEntity));
        skill.Spells.AddRange(dbObject.Spells.Select(SpellsDAL.ConvertDbObjectToEntity));

        return skill;
    }
}
