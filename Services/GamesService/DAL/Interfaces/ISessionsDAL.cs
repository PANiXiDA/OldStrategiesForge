using BaseDAL;
using Common.ConvertParams.GamesService;
using Common.SearchParams.GamesService;
using GamesService.DAL.DbModels;
using GamesService.DAL.DbModels.Models;
using GamesService.Dto;

namespace GamesService.DAL.Interfaces;

public interface ISessionsDAL : IBaseDal<DefaultDbContext, Session, SessionDto, Guid, SessionsSearchParams, SessionsConvertParams>
{
}
