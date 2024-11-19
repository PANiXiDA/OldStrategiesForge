using BaseDAL;
using Common.ConvertParams.PlayersService;
using Common.SearchParams.PlayersService;
using ProfileService.DAL.DbModels;
using ProfileService.DAL.DbModels.Models;
using ProfileService.Dto.Players;

namespace ProfileService.DAL.Interfaces;

public interface IPlayersDAL : IBaseDal<DefaultDbContext, Player, PlayersDto, int, PlayersSearchParams, PlayersConvertParams>
{
    Task<bool> ExistsAsync(string email);
    Task<PlayersDto?> GetAsync(string email);
}
