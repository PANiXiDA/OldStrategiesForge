using BaseDAL;
using Common.ConvertParams.PlayersService;
using Common.SearchParams.PlayersService;
using ProfileDatabaseService.DAL.DbModels;
using ProfileDatabaseService.DAL.DbModels.Models;
using ProfileDatabaseService.Dto.Players;

namespace ProfileDatabaseService.DAL.Interfaces;

public interface IPlayersDAL : IBaseDal<DefaultDbContext, Player, PlayersDto, int, PlayersSearchParams, PlayersConvertParams>
{
}
