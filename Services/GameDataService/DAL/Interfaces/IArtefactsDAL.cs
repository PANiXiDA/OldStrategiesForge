using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using ArtefactDb = GameDataService.DAL.DbModels.Models.Artefact;

namespace GameDataService.DAL.Interfaces;

public interface IArtefactsDAL : IBaseDal<DefaultDbContext, ArtefactDb, Artefact, int, ArtefactsSearchParams, ArtefactsConvertParams>
{
}
