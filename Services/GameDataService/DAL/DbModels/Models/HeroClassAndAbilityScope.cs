using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class HeroClassAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("hero_class_id")]
    public int HeroClassId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual HeroClass? HeroClass { get; set; }
    public virtual Ability? Ability { get; set; }
}
