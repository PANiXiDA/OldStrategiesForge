using BaseDAL;
using Common.ConvertParams.PlayersService;
using Common.SearchParams.PlayersService;
using ProfileDatabaseService.DAL.DbModels;
using ProfileDatabaseService.DAL.DbModels.Moddels;

namespace ProfileDatabaseService.DAL.Interfaces;

public interface IPlayersDAL : IBaseDal<DefaultDbContext, Player, object, int, PlayersSearchParams, PlayersConvertParams>
{
}
