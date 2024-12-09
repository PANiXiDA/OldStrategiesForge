using BaseDAL;
using Common.ConvertParams.ProfileService;
using Common.SearchParams.ProfileService;
using ProfileService.DAL.DbModels;
using ProfileService.DAL.DbModels.Models;
using ProfileService.Dto;

namespace ProfileService.DAL.Interfaces;

public interface IPlayersDAL : IBaseDal<DefaultDbContext, Player, PlayersDto, int, PlayersSearchParams, PlayersConvertParams>
{
    Task<bool> ExistsAsync(string email);
    Task<PlayersDto?> GetAsync(string email);
    Task UpdateLastLogin(int id);
}
