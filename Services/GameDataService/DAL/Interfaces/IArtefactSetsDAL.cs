using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using ArtefactSetDb = GameDataService.DAL.DbModels.Models.ArtefactSet;

namespace GameDataService.DAL.Interfaces;

public interface IArtefactSetsDAL : IBaseDal<DefaultDbContext, ArtefactSetDb, ArtefactSet, int, ArtefactSetsSearchParams, ArtefactSetsConvertParams>
{
}
