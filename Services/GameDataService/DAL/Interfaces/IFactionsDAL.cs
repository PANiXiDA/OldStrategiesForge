using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using FactionDb = GameDataService.DAL.DbModels.Models.Faction;

namespace GameDataService.DAL.Interfaces;

public interface IFactionsDAL : IBaseDal<DefaultDbContext, FactionDb, Faction, int, FactionsSearchParams, FactionsConvertParams>
{
}
