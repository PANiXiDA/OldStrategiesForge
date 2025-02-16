using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using HeroClassDb = GameDataService.DAL.DbModels.Models.HeroClass;

namespace GameDataService.DAL.Interfaces;

public interface IHeroClassesDAL : IBaseDal<DefaultDbContext, HeroClassDb, HeroClass, int, HeroClassesSearchParams, HeroClassesConvertParams>
{
}
