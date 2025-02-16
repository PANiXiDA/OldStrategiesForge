using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using EffectDb = GameDataService.DAL.DbModels.Models.Effect;

namespace GameDataService.DAL.Interfaces;

public interface IEffectsDAL : IBaseDal<DefaultDbContext, EffectDb, Effect, int, EffectsSearchParams, EffectsConvertParams>
{
}
