using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using CompetenceDb = GameDataService.DAL.DbModels.Models.Competence;

namespace GameDataService.DAL.Implementations;

public class CompetenciesDAL : BaseDAL<DefaultDbContext, CompetenceDb,
    Competence, int, CompetenciesSearchParams, CompetenciesConvertParams>, ICompetenciesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public CompetenciesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, Competence entity,
        CompetenceDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.SubfactionId = entity.SubfactionId;

        return Task.CompletedTask;
    }

    protected override IQueryable<CompetenceDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<CompetenceDb> dbObjects, CompetenciesSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<Competence>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<CompetenceDb> dbObjects, CompetenciesConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<CompetenceDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<Competence, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static Competence ConvertDbObjectToEntity(CompetenceDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var competence = new Competence()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description,
            SubfactionId = dbObject.SubfactionId,

            Subfaction = dbObject.Subfaction != null ? SubfactionsDAL.ConvertDbObjectToEntity(dbObject.Subfaction) : null
        };

        competence.Skills.AddRange(dbObject.Skills.Select(SkillsDAL.ConvertDbObjectToEntity));

        return competence;
    }
}
