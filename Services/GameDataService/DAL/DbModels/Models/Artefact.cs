using Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Artefact
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("attack_bonus")]
    public int? AttackBonus { get; set; }

    [Column("defence_bonus")]
    public int? DefenceBonus { get; set; }

    [Column("health_bonus")]
    public int? HealthBonus { get; set; }

    [Column("min_damage_bonus")]
    public int? MinDamageBonus { get; set; }

    [Column("max_damage_bonus")]
    public int? MaxDamageBonus { get; set; }

    [Column("initiative_bonus")]
    public double? InitiativeBonus { get; set; }

    [Column("speed_bonus")]
    public int? SpeedBonus { get; set; }

    [Column("range_bonus")]
    public int? RangeBonus { get; set; }

    [Column("arrows_bonus")]
    public int? ArrowsBonus { get; set; }

    [Column("morale_bonus")]
    public int? MoraleBonus { get; set; }

    [Column("luck_bonus")]
    public int? LuckBonus { get; set; }

    [Column("artifact_slot")]
    public ArtifactSlot ArtifactSlot { get; set; }

    [Column("hero_class_id")]
    public int? HeroClassId { get; set; }

    [Column("artefact_set_id")]
    public int? ArtefactSetId { get; set; }

    public virtual HeroClass? HeroClass { get; set; }
    public virtual ArtefactSet? ArtefactSet { get; set; }

    public virtual ICollection<ArtefactAndAbilityScope> ArtefactAndAbilityScopes { get; set; } = new HashSet<ArtefactAndAbilityScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => ArtefactAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
