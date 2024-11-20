using BaseDAL;
using Common.ConvertParams.ProfileService;
using ProfileService.DAL.DbModels.Models;
using ProfileService.DAL.DbModels;
using ProfileService.Dto;
using Common.SearchParams.ProfileService;

namespace ProfileService.DAL.Interfaces;

public interface ITokensDAL : IBaseDal<DefaultDbContext, Token, TokensDto, int, TokensSearchParams, TokensConvertParams>
{
    Task<bool> ExistsAsync(string refreshToken);
    Task<TokensDto?> GetAsync(string refreshToken);
}
