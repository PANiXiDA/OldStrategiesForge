using BaseDAL;
using GameData.ConvertParams.Gen;
using GameData.Entities.Gen;
using GameData.SearchParams.Gen;
using GameDataService.DAL.DbModels;
using SpellDb = GameDataService.DAL.DbModels.Models.Spell;

namespace GameDataService.DAL.Interfaces;

public interface ISpellsDAL : IBaseDal<DefaultDbContext, SpellDb, Spell, int, SpellsSearchParams, SpellsConvertParams>
{
}
