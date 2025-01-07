using BaseDAL;
using Common.ConvertParams.GamesService;
using Common.SearchParams.GamesService;
using GamesService.DAL.DbModels;
using GamesService.DAL.DbModels.Models;
using GamesService.Dto;

namespace GamesService.DAL.Interfaces;

public interface IGamesDAL : IBaseDal<DefaultDbContext, Game, GameDto, Guid, GamesSearchParams, GamesConvertParams>
{
}
