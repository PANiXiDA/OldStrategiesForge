using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using CompetenceDb = GameDataService.DAL.DbModels.Models.Competence;

namespace GameDataService.DAL.Interfaces;

public interface ICompetenciesDAL : IBaseDal<DefaultDbContext, CompetenceDb, Competence, int, CompetenciesSearchParams, CompetenciesConvertParams>
{
}
