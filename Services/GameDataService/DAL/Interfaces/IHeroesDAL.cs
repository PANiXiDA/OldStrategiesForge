using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using HeroDb = GameDataService.DAL.DbModels.Models.Hero;

namespace GameDataService.DAL.Interfaces;

public interface IHeroesDAL : IBaseDal<DefaultDbContext, HeroDb, Hero, int, HeroesSearchParams, HeroesConvertParams>
{
}
