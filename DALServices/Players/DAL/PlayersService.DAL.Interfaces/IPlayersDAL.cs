using BaseDAL;
using Common.ConvertParams.PlayersService;
using Common.SearchParams.PlayersService;
using PlayersService.DAL.DbModels;
using PlayersService.DAL.DbModels.Models;

namespace PlayersService.DAL.Interfaces;
public interface IPlayersDAL : IBaseDal<DefaultDbContext, Player, object, int, PlayersSearchParams, PlayersConvertParams>
{
}
