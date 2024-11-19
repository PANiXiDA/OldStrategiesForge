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

internal class PlayersDAL : BaseDAL<DefaultDbContext, Player,
    PlayersDto, int, PlayersSearchParams, PlayersConvertParams>, IPlayersDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public PlayersDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, PlayersDto entity,
        Player dbObject, bool exists)
    {
        dbObject.CreatedAt = entity.CreatedAt;
        dbObject.UpdatedAt = entity.UpdatedAt;
        dbObject.DeletedAt = entity.DeletedAt;
        dbObject.Email = entity.Email;
        dbObject.Password = entity.Password;
        dbObject.Nickname = entity.Nickname;
        dbObject.Confirmed = entity.Confirmed;
        dbObject.Blocked = entity.Blocked;
        dbObject.Role = entity.Role;
        dbObject.LastLogin = entity.LastLogin;
        dbObject.AvatarId = entity.AvatarId;
        dbObject.FrameId = entity.FrameId;
        dbObject.Games = entity.Games;
        dbObject.Wins = entity.Wins;
        dbObject.Loses = entity.Loses;
        dbObject.Mmr = entity.Mmr;
        dbObject.Rank = entity.Rank;
        dbObject.Premium = entity.Premium;
        dbObject.Gold = entity.Gold;
        dbObject.Level = entity.Level;
        dbObject.Experience = entity.Experience;

        return Task.CompletedTask;
    }

    protected override IQueryable<Player> BuildDbQuery(DefaultDbContext context,
        IQueryable<Player> dbObjects, PlayersSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Email))
        {
            dbObjects = dbObjects.Where(item => item.Email.ToLower() == searchParams.Email.ToLower());
        }
        if (!string.IsNullOrEmpty(searchParams.Nickname))
        {
            dbObjects = dbObjects.Where(item => item.Nickname.ToLower() == searchParams.Nickname.ToLower());
        }

        return dbObjects;
    }

    protected override async Task<IList<PlayersDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Player> dbObjects, PlayersConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Player, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<PlayersDto, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static PlayersDto ConvertDbObjectToEntity(Player dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new PlayersDto()
        {
            Id = dbObject.Id,
            CreatedAt = dbObject.CreatedAt,
            UpdatedAt = dbObject.UpdatedAt,
            DeletedAt = dbObject.DeletedAt,
            Email = dbObject.Email,
            Password = dbObject.Password,
            Nickname = dbObject.Nickname,
            Confirmed = dbObject.Confirmed,
            Blocked = dbObject.Blocked,
            Role = dbObject.Role,
            LastLogin = dbObject.LastLogin,
            AvatarId = dbObject.AvatarId,
            FrameId = dbObject.FrameId,
            Games = dbObject.Games,
            Wins = dbObject.Wins,
            Loses = dbObject.Loses,
            Mmr = dbObject.Mmr,
            Rank = dbObject.Rank,
            Premium = dbObject.Premium,
            Gold = dbObject.Gold,
            Level = dbObject.Level,
            Experience = dbObject.Experience
        };
    }

    public Task<bool> ExistsAsync(string email)
    {
        return ExistsAsync(item => item.Email == email);
    }

    public async Task<PlayersDto?> GetAsync(string email)
    {
        return (await GetAsync(item => item.Email == email)).FirstOrDefault();
    }
}

