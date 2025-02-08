using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class ArtefactSetBonusAndAbilityScope
{
    [Column("id")]
    public int Id { get; set; }

    [Column("artefact_set_bonus_id")]
    public int ArtefactSetBonusId { get; set; }

    [Column("ability_id")]
    public int AbilityId { get; set; }

    public virtual ArtefactSetBonus? ArtefactSetBonus { get; set; }
    public virtual Ability? Ability { get; set; }
}
