using BaseDAL;
using Games.ConvertParams.Gen;
using Games.Entities.Gen;
using Games.SearchParams.Gen;
using GamesService.DAL.DbModels;
using GameDb = GamesService.DAL.DbModels.Models.Game;

namespace GamesService.DAL.Interfaces;

public interface IGamesDAL : IBaseDal<DefaultDbContext, GameDb, Game, Guid, GamesSearchParams, GamesConvertParams>
{
    Task CloseAsync(Guid id);
}
