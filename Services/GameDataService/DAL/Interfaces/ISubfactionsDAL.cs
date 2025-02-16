using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using SubfactionDb = GameDataService.DAL.DbModels.Models.Subfaction;

namespace GameDataService.DAL.Interfaces;

public interface ISubfactionsDAL : IBaseDal<DefaultDbContext, SubfactionDb, Subfaction, int, SubfactionsSearchParams, SubfactionsConvertParams>
{
}
