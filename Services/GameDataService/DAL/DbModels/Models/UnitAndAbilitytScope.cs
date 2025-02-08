using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class UnitAndAbilitytScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("unit_id")]
    public int UnitId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual Unit? Unit { get; set; }
    public virtual Ability? Ability { get; set; }
}
