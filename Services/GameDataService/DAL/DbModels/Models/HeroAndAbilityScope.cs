using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class HeroAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("hero_id")]
    public int HeroId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual Hero? Hero { get; set; }
    public virtual Ability? Ability { get; set; }
}
