using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using AbilityDb = GameDataService.DAL.DbModels.Models.Ability;

namespace GameDataService.DAL.Interfaces;

public interface IAbilitiesDAL : IBaseDal<DefaultDbContext, AbilityDb, Ability, int, AbilitiesSearchParams, AbilitiesConvertParams>
{
}
