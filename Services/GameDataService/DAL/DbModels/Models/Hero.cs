using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Hero
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("attack")]
    public int Attack { get; set; }

    [Column("defence")]
    public int Defence { get; set; }

    [Column("min_damage")]
    public int MinDamage { get; set; }

    [Column("max_damage")]
    public int MaxDamage { get; set; }

    [Column("initiative")]
    public double Initiative { get; set; }

    [Column("morale")]
    public int Morale { get; set; }

    [Column("luck")]
    public int Luck { get; set; }

    [Column("subfaction_id")]
    public int SubfactionId { get; set; }

    [Column("hero_class_id")]
    public int HeroClassId { get; set; }

    public virtual Subfaction? Subfaction { get; set; }
    public virtual HeroClass? HeroClass { get; set; }
    public virtual ICollection<HeroAndAbilityScope> HeroAndAbilityScopes { get; set; } = new HashSet<HeroAndAbilityScope>();
    public virtual ICollection<HeroAndArtefactScope> HeroAndArtefactScopes { get; set; } = new HashSet<HeroAndArtefactScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => HeroAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);

    [NotMapped]
    public IEnumerable<Artefact> Artefacts => HeroAndArtefactScopes
        .Where(item => item.Artefact != null)
        .Select(item => item.Artefact!);
}
