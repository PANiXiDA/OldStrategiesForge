using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using SkillDb = GameDataService.DAL.DbModels.Models.Skill;

namespace GameDataService.DAL.Interfaces;

public interface ISkillsDAL : IBaseDal<DefaultDbContext, SkillDb, Skill, int, SkillsSearchParams, SkillsConvertParams>
{
}
