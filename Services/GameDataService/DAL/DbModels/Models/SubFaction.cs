using System.ComponentModel.DataAnnotations.Schema;

namespace GameDataService.DAL.DbModels.Models;

public class Subfaction
{
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("faction_id")]
    public int FactionId { get; set; }

    public virtual Faction? Faction { get; set; }
    public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();

    public virtual ICollection<SubfactionAndAbilityScope> SubfactionAndAbilityScopes { get; set; } = new HashSet<SubfactionAndAbilityScope>();

    [NotMapped]
    public IEnumerable<Ability> Abilities => SubfactionAndAbilityScopes
        .Where(item => item.Ability != null)
        .Select(item => item.Ability!);
}
