using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using ArtefactSetBonusDb = GameDataService.DAL.DbModels.Models.ArtefactSetBonus;

namespace GameDataService.DAL.Interfaces;

public interface IArtefactSetBonusesDAL : IBaseDal<DefaultDbContext, ArtefactSetBonusDb, ArtefactSetBonus, int, ArtefactSetBonusesSearchParams, ArtefactSetBonusesConvertParams>
{
}
