using BaseDAL;
using Common.ConvertParams.ProfileService;
using Common.SearchParams.ProfileService;
using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.DbModels.Models;
using ProfileService.DAL.DbModels;
using ProfileService.DAL.Interfaces;
using ProfileService.Dto;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace ProfileService.DAL.Implementations;

internal class AvatarsDAL : BaseDAL<DefaultDbContext, Avatar,
    AvatarsDto, int, AvatarsSearchParams, AvatarsConvertParams>, IAvatarsDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public AvatarsDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, AvatarsDto entity,
        Avatar dbObject, bool exists)
    {
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = entity.UpdatedAt;
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

    protected override IQueryable<Avatar> BuildDbQuery(DefaultDbContext context,
        IQueryable<Avatar> dbObjects, AvatarsSearchParams searchParams)
    {
        return dbObjects;
    }

    protected override async Task<IList<AvatarsDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Avatar> dbObjects, AvatarsConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Avatar, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<AvatarsDto, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static AvatarsDto ConvertDbObjectToEntity(Avatar dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new AvatarsDto(
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
