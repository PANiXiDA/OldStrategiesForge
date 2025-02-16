using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using UnitDb = GameDataService.DAL.DbModels.Models.Unit;

namespace GameDataService.DAL.Interfaces;

public interface IUnitsDAL : IBaseDal<DefaultDbContext, UnitDb, Unit, int, UnitsSearchParams, UnitsConvertParams>
{
}
