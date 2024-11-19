using Common.ConvertParams.ProfileService;
using Common.SearchParams.ProfileService;
using Microsoft.EntityFrameworkCore;
using ProfileService.DAL.DbModels.Models;
using ProfileService.DAL.DbModels;
using ProfileService.Dto;
using System.Linq.Expressions;
using BaseDAL;
using ProfileService.DAL.Interfaces;

namespace ProfileService.DAL.Implementations;

public class TokensDAL : BaseDAL<DefaultDbContext, Token,
    TokensDto, int, TokensSearchParams, TokensConvertParams>, ITokensDAL
{
    protected override bool RequiresUpdatesAfterObjectSaving => false;

    public TokensDAL(DefaultDbContext context) : base(context) { }

    protected override Task UpdateBeforeSavingAsync(DefaultDbContext context, TokensDto entity,
        Token dbObject, bool exists)
    {
        dbObject.PlayerId = entity.PlayerId;
        dbObject.RefreshToken = entity.RefreshToken;
        dbObject.RefreshTokenExp = entity.RefreshTokenExp;

        return Task.CompletedTask;
    }

    protected override IQueryable<Token> BuildDbQuery(DefaultDbContext context,
        IQueryable<Token> dbObjects, TokensSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.RefreshToken))
        {
            dbObjects = dbObjects.Where(item => item.RefreshToken == searchParams.RefreshToken);
        }
        if (searchParams.PlayerId.HasValue)
        {
            dbObjects = dbObjects.Where(item => item.PlayerId == searchParams.PlayerId.Value);
        }

        return dbObjects;
    }

    protected override async Task<IList<TokensDto>> BuildEntitiesListAsync(DefaultDbContext context,
        IQueryable<Token> dbObjects, TokensConvertParams? convertParams, bool isFull)
    {
        return (await dbObjects.ToListAsync()).Select(ConvertDbObjectToEntity).ToList();
    }

    protected override Expression<Func<Token, int>> GetIdByDbObjectExpression()
    {
        return item => item.Id;
    }

    protected override Expression<Func<TokensDto, int>> GetIdByEntityExpression()
    {
        return item => item.Id;
    }

    internal static TokensDto ConvertDbObjectToEntity(Token dbObject)
    {
        if (dbObject == null) throw new ArgumentNullException(nameof(dbObject));

        return new TokensDto()
        {
            Id = dbObject.Id,
            PlayerId = dbObject.PlayerId,
            RefreshToken = dbObject.RefreshToken,
            RefreshTokenExp = dbObject.RefreshTokenExp
        };
    }
}
