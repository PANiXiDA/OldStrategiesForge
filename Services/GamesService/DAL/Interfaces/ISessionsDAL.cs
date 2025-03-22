using BaseDAL;
using Games.ConvertParams.Gen;
using Games.Entities.Gen;
using Games.SearchParams.Gen;
using GamesService.DAL.DbModels;
using SessionDb = GamesService.DAL.DbModels.Models.Session;

namespace GamesService.DAL.Interfaces;

public interface ISessionsDAL : IBaseDal<DefaultDbContext, SessionDb, Session, Guid, SessionsSearchParams, SessionsConvertParams>
{
}
