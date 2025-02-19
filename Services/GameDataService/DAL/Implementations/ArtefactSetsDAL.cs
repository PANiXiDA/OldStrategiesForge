using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using GameDataService.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ArtefactSetDb = GameDataService.DAL.DbModels.Models.ArtefactSet;

namespace GameDataService.DAL.Implementations;

public class ArtefactSetsDAL : BaseDAL<DefaultDbContext, ArtefactSetDb,
    ArtefactSet, int, ArtefactSetsSearchParams, ArtefactSetsConvertParams>, IArtefactSetsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public ArtefactSetsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, ArtefactSet entity,
        ArtefactSetDb dbObject, bool exists)
    {
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;

        return Task.CompletedTask;
    }

    protected override IQueryable<ArtefactSetDb> BuildDbQuery(DefaultDbContext context,
        IQueryable<ArtefactSetDb> dbObjects, ArtefactSetsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<ArtefactSet>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<ArtefactSetDb> dbObjects, ArtefactSetsConvertParams? convertParams, bool isFull)
    {
        if (convertParams != null)
        {
            if (convertParams.HasIncludeArtefactSetBonus && convertParams.IncludeArtefactSetBonus)
            {
                dbObjects = dbObjects.Include(item => item.ArtefactSetBonuses);
            }
            if (convertParams.HasIncludeArtefacts && convertParams.IncludeArtefacts)
            {
                dbObjects = dbObjects.Include(item => item.Artefacts);
            }
        }

        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<ArtefactSetDb, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<ArtefactSet, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static ArtefactSet ConvertDbObjectToEntity(ArtefactSetDb dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        var artefactSet = new ArtefactSet()
        {
            Id = dbObject.Id,
            Name = dbObject.Name,
            Description = dbObject.Description
        };

        artefactSet.ArtefactSetBonuses.AddRange(dbObject.ArtefactSetBonuses.Select(ArtefactSetBonusesDAL.ConvertDbObjectToEntity));
        artefactSet.Artefacts.AddRange(dbObject.Artefacts.Select(ArtefactsDAL.ConvertDbObjectToEntity));

        return artefactSet;
    }
}
