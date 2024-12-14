using BaseDAL;
using Common.ConvertParams.ProfileService;
using Common.SearchParams.ProfileService;
using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.DbModels;
using ProfileService.DAL.DbModels.Models;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto;
using System.Linq.Expressions;

namespace ProfileService.DAL.Implementations;

internal class FramesDAL : BaseDAL<DefaultDbContext, Frame,
    FramesDto, int, FramesSearchParams, FramesConvertParams>, IFramesDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public FramesDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, FramesDto entity,
        Frame dbObject, bool exists)
    {
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = DateTime.UtcNow;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.S3Path = entity.S3Path;
        dbObject.Name = entity.Name;
        dbObject.Description = entity.Description;
        dbObject.NecessaryMmr = entity.NecessaryMmr;
        dbObject.NecessaryGames = entity.NecessaryGames;
        dbObject.NecessaryWins = entity.NecessaryWins;
        dbObject.Available = entity.Available;

        return Task.CompletedTask;
    }

    protected override IQueryable<Frame> BuildDbQuery(DefaultDbContext context,
        IQueryable<Frame> dbObjects, FramesSearchParams searchParams)
    {
        if (searchParams.IsAvailable.HasValue)
        {
            dbObjects = dbObjects.Where(item => item.Available == searchParams.IsAvailable.Value);
        }

        return dbObjects;
    }

    protected override async Task<IList<FramesDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Frame> dbObjects, FramesConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Frame, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<FramesDto, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static FramesDto ConvertDbObjectToEntity(Frame dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new FramesDto(
            dbObject.Id,
            dbObject.S3Path,
            dbObject.Name,
            dbObject.Description,
            dbObject.NecessaryMmr,
            dbObject.NecessaryGames,
            dbObject.NecessaryWins,
            dbObject.Available)
        {
            CreatedAt = dbObject.CreatedAt,
            UpdatedAt = dbObject.UpdatedAt,
            DeletedAt = dbObject.DeletedAt,
        };
    }
}
